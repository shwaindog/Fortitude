// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv

#region

using System.Collections;

#endregion

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Common;

public enum SwitchType
{
    Simple
    , PostMinus
    , LimitedPostString
    , UnLimitedPostString
    , PostChar
}

public class SwitchForm
{
    public string IDString;
    public int MaxLen;
    public int MinLen;
    public bool Multi;
    public string PostCharSet;
    public SwitchType Type;

    public SwitchForm(string idString, SwitchType type, bool multi,
        int minLen, int maxLen, string postCharSet)
    {
        IDString = idString;
        Type = type;
        Multi = multi;
        MinLen = minLen;
        MaxLen = maxLen;
        PostCharSet = postCharSet;
    }

    public SwitchForm(string idString, SwitchType type, bool multi, int minLen) :
        this(idString, type, multi, minLen, 0, "") { }

    public SwitchForm(string idString, SwitchType type, bool multi) :
        this(idString, type, multi, 0) { }
}

public class SwitchResult
{
    public int PostCharIndex;
    public ArrayList PostStrings = new();
    public bool ThereIs;
    public bool WithMinus;

    public SwitchResult() => ThereIs = false;
}

public class Parser
{
    private const char SwitchID1 = '-';
    private const char SwitchID2 = '/';

    private const char SwitchMinus = '-';
    private const string StopSwitchParsing = "--";
    private SwitchResult[] switches;
    public ArrayList NonSwitchStrings = new();

    public Parser(int numSwitches)
    {
        switches = new SwitchResult[numSwitches];
        for (var i = 0; i < numSwitches; i++)
            switches[i] = new SwitchResult();
    }

    public SwitchResult this[int index] => switches[index];

    private bool ParseString(string srcString, SwitchForm[] switchForms)
    {
        var len = srcString.Length;
        if (len == 0)
            return false;
        var pos = 0;
        if (!IsItSwitchChar(srcString[pos]))
            return false;
        while (pos < len)
        {
            if (IsItSwitchChar(srcString[pos]))
                pos++;
            const int NoLen = -1;
            var matchedSwitchIndex = 0;
            var maxLen = NoLen;
            for (var switchIndex = 0; switchIndex < switches.Length; switchIndex++)
            {
                var switchLen = switchForms[switchIndex].IDString.Length;
                if (switchLen <= maxLen || pos + switchLen > len)
                    continue;
                if (string.Compare(switchForms[switchIndex].IDString, 0,
                        srcString, pos, switchLen, true) == 0)
                {
                    matchedSwitchIndex = switchIndex;
                    maxLen = switchLen;
                }
            }

            if (maxLen == NoLen)
                throw new Exception("maxLen == kNoLen");
            var matchedSwitch = switches[matchedSwitchIndex];
            var switchForm = switchForms[matchedSwitchIndex];
            if (!switchForm.Multi && matchedSwitch.ThereIs)
                throw new Exception("switch must be single");
            matchedSwitch.ThereIs = true;
            pos += maxLen;
            var tailSize = len - pos;
            var type = switchForm.Type;
            switch (type)
            {
                case SwitchType.PostMinus:
                {
                    if (tailSize == 0)
                    {
                        matchedSwitch.WithMinus = false;
                    }
                    else
                    {
                        matchedSwitch.WithMinus = srcString[pos] == SwitchMinus;
                        if (matchedSwitch.WithMinus)
                            pos++;
                    }

                    break;
                }
                case SwitchType.PostChar:
                {
                    if (tailSize < switchForm.MinLen)
                        throw new Exception("switch is not full");
                    var charSet = switchForm.PostCharSet;
                    const int kEmptyCharValue = -1;
                    if (tailSize == 0)
                    {
                        matchedSwitch.PostCharIndex = kEmptyCharValue;
                    }
                    else
                    {
                        var index = charSet.IndexOf(srcString[pos]);
                        if (index < 0)
                        {
                            matchedSwitch.PostCharIndex = kEmptyCharValue;
                        }
                        else
                        {
                            matchedSwitch.PostCharIndex = index;
                            pos++;
                        }
                    }

                    break;
                }
                case SwitchType.LimitedPostString:
                case SwitchType.UnLimitedPostString:
                {
                    var minLen = switchForm.MinLen;
                    if (tailSize < minLen)
                        throw new Exception("switch is not full");
                    if (type == SwitchType.UnLimitedPostString)
                    {
                        matchedSwitch.PostStrings.Add(srcString.Substring(pos));
                        return true;
                    }

                    var stringSwitch = srcString.Substring(pos, minLen);
                    pos += minLen;
                    for (var i = minLen; i < switchForm.MaxLen && pos < len; i++, pos++)
                    {
                        var c = srcString[pos];
                        if (IsItSwitchChar(c))
                            break;
                        stringSwitch += c;
                    }

                    matchedSwitch.PostStrings.Add(stringSwitch);
                    break;
                }
            }
        }

        return true;
    }

    public void ParseStrings(SwitchForm[] switchForms, string[] commandStrings)
    {
        var numCommandStrings = commandStrings.Length;
        var stopSwitch = false;
        for (var i = 0; i < numCommandStrings; i++)
        {
            var s = commandStrings[i];
            if (stopSwitch)
                NonSwitchStrings.Add(s);
            else if (s == StopSwitchParsing)
                stopSwitch = true;
            else if (!ParseString(s, switchForms))
                NonSwitchStrings.Add(s);
        }
    }

    public static int ParseCommand(CommandForm[] commandForms, string commandString,
        out string postString)
    {
        for (var i = 0; i < commandForms.Length; i++)
        {
            var id = commandForms[i].IDString;
            if (commandForms[i].PostStringMode)
            {
                if (commandString.IndexOf(id) == 0)
                {
                    postString = commandString.Substring(id.Length);
                    return i;
                }
            }
            else if (commandString == id)
            {
                postString = "";
                return i;
            }
        }

        postString = "";
        return -1;
    }

    private static bool ParseSubCharsCommand(int numForms, CommandSubCharsSet[] forms,
        string commandString, ArrayList indices)
    {
        indices.Clear();
        var numUsedChars = 0;
        for (var i = 0; i < numForms; i++)
        {
            var charsSet = forms[i];
            var currentIndex = -1;
            var len = charsSet.Chars.Length;
            for (var j = 0; j < len; j++)
            {
                var c = charsSet.Chars[j];
                var newIndex = commandString.IndexOf(c);
                if (newIndex >= 0)
                {
                    if (currentIndex >= 0)
                        return false;
                    if (commandString.IndexOf(c, newIndex + 1) >= 0)
                        return false;
                    currentIndex = j;
                    numUsedChars++;
                }
            }

            if (currentIndex == -1 && !charsSet.EmptyAllowed)
                return false;
            indices.Add(currentIndex);
        }

        return numUsedChars == commandString.Length;
    }

    private static bool IsItSwitchChar(char c) => c == SwitchID1 || c == SwitchID2;
}

public class CommandForm
{
    public string IDString = "";
    public bool PostStringMode = false;

    public CommandForm(string idString, bool postStringMode)
    {
        IDString = idString;
        PostStringMode = postStringMode;
    }
}

internal class CommandSubCharsSet
{
    public string Chars = "";
    public bool EmptyAllowed = false;
}
