// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ConvertToPrimaryConstructor

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData;

[NoMatchingProductionClass]
public class SchoolForUnrelatedStudiesTestData
{
    public static StringStyle DefaultStyle = StringStyle.Json;


    private readonly ICharSequence eidShakespeare   = new CharArrayStringBuilder("EId_Shakespeare");
    private readonly ICharSequence eidDiesel        = new CharArrayStringBuilder("EId_Diesel");
    private readonly ICharSequence eidJoel          = new CharArrayStringBuilder("EId_Joel");
    private readonly ICharSequence eidLennon        = new CharArrayStringBuilder("EId_Lennon");
    private readonly ICharSequence eidSchwarznegger = new CharArrayStringBuilder("EId_Schwarznegger");
    private readonly ICharSequence eidGroot         = new CharArrayStringBuilder("EId_Groot");

    private readonly ICharSequence sidShatner  = new MutableString("sid_Shatner");
    private readonly ICharSequence sidWilliams = new MutableString("sid_Williams");
    private readonly ICharSequence sidPitt     = new MutableString("sid_Pitt");
    private readonly ICharSequence sidConnery  = new MutableString("sid_Connery");
    private readonly ICharSequence sidSimpson  = new MutableString("sid_Simpson");
    private readonly ICharSequence sidCostanza = new MutableString("sid_Costanza");
    private readonly ICharSequence sidZoidberg = new MutableString("sid_Zoidberg");
    public List<KeyValuePair<FacultyType, Faculty>> Faculties { get; set; } = null!;

    public Dictionary<ICharSequence, EducationAttendee> Students { get; set; } = null!;

    public Dictionary<ICharSequence, CourseDeliverer> CourseDeliverers { get; set; } = null!;

    public Dictionary<string, CourseSubject> Subjects { get; set; } = null!;


    public AccredittedInstructor Shakespeare => (AccredittedInstructor)CourseDeliverers[eidShakespeare];
    public Lecturer Diesel => (Lecturer)CourseDeliverers[eidDiesel];
    public TradesInstructor Joel => (TradesInstructor)CourseDeliverers[eidJoel];
    public Lecturer Lennon => (Lecturer)CourseDeliverers[eidLennon];
    public Lecturer Schwarznegger => (Lecturer)CourseDeliverers[eidSchwarznegger];
    public Lecturer Groot => (Lecturer)CourseDeliverers[eidGroot];

    public Student Shatner => (Student)Students[sidShatner];
    public Student Williams => (Student)Students[sidWilliams];
    public Student Pitt => (Student)Students[sidPitt];
    public Student Connery => (Student)Students[sidConnery];
    public Student Simpson => (Student)Students[sidSimpson];
    public Student Costanza => (Student)Students[sidCostanza];
    public Student Zoidberg => (Student)Students[sidZoidberg];

    // ReSharper disable InconsistentNaming
    public ArtsSubject LA101 => (ArtsSubject)Subjects["LA101"];
    public ArtsSubject FA102 => (ArtsSubject)Subjects["FA102"];
    public ArtsSubject AH103 => (ArtsSubject)Subjects["AH103"];
    public ArtsSubject LA111 => (ArtsSubject)Subjects["LA111"];
    public ArtsSubject FA111 => (ArtsSubject)Subjects["FA111"];
    public ArtsSubject AH111 => (ArtsSubject)Subjects["AH111"];
    public TradesSubject EL101 => (TradesSubject)Subjects["EL101"];
    public EngineeringSubject MA102 => (EngineeringSubject)Subjects["MA102"];
    public TradesSubject PB101 => (TradesSubject)Subjects["PB101"];
    public TradesSubject EL102 => (TradesSubject)Subjects["EL102"];
    public EngineeringSubject MA103 => (EngineeringSubject)Subjects["MA103"];
    public TradesSubject PB102 => (TradesSubject)Subjects["PB102"];
    // ReSharper restore InconsistentNaming


    public TradeSkills ElectricianSchool => (TradeSkills)Faculties.Single(kvp => kvp.Key == FacultyType.ElectricianSchool).Value;
    public ArtsFaculty Literature => (ArtsFaculty)Faculties.Single(kvp => kvp.Key == FacultyType.Literature).Value;
    public TradeSkills PlumbingSchool => (TradeSkills)Faculties.Single(kvp => kvp.Key == FacultyType.PlumbingSchool).Value;
    public Engineering AeronauticalEngineering => (Engineering)Faculties.Single(kvp => kvp.Key == FacultyType.AeronauticalEngineering).Value;
    public ArtsFaculty FineArts => (ArtsFaculty)Faculties.Single(kvp => kvp.Key == FacultyType.FineArts).Value;
    public ArtsFaculty AncientGreekPhilosophy => (ArtsFaculty)Faculties.Single(kvp => kvp.Key == FacultyType.AncientGreekPhilosophy).Value;

    public SchoolForUnrelatedStudiesTestData()
    {
        Populate();
    }

    private void Populate()
    {
        CourseDeliverers = new Dictionary<ICharSequence, CourseDeliverer>()
        {
            {
                eidShakespeare, new AccredittedInstructor
                {
                    EmployeeId     = eidShakespeare
                  , DateOfBirth    = new DateTime(1564, 4, 23)
                  , FirstName      = "Billy"
                  , LastName       = new MutableString("Shakespeare")
                  , UniquePersonId = 1
                }.AddLicense(new HighVoltageElectriciansLicense("CrossingTheBeamsIsBad")
                {
                    LicenseNumber  = 708304231
                  , ExpirationDate = new DateTime(1591, 10, 10)
                  , PhotoLocation
                        = new Uri("https://en.wikipedia.org/wiki/Portraits_of_Shakespeare#/media/File:Ashbourne_portrait_ShakespeareHamersley.jpg")
                  , Issued        = new DateTime(1582, 10, 10)
                  , GradeOfAccess = LicenseGrade.Level5
                }).AddLicense(new AutomobileLicense
                {
                    LicenseNumber  = 1
                  , ExpirationDate = new DateTime(1592, 5, 22)
                  , CurrentRestrictions =
                    [
                        new Restrictions(TimeSpan.FromDays(365))
                        {
                            Name = "Requires Glases"
                          , Type = new StringBuilder("Vision")
                          , SubRestrictionsApplied =
                            [
                                new KeyValuePair<DateTime, SubRestrictions>
                                    (new DateTime(1595, 9, 2)
                                   , new SubRestrictions
                                     {
                                         Enforcable = false
                                       , SubDescription = JsonSerializer.Deserialize<JsonObject>
                                             ("""
                                              { 
                                                "Name": "NightTimeDriving",
                                                "Description": "Not allowed night time driving"
                                              }
                                              """
                                             )!
                                     }
                                    )
                              , new KeyValuePair<DateTime, SubRestrictions>
                                    (new DateTime(1597, 2, 14)
                                   , new SubRestrictions
                                     {
                                         Enforcable = true
                                       , SubDescription = JsonSerializer.Deserialize<JsonObject>
                                             ("""
                                              { 
                                                "Name": "Blood Alcohol",
                                                "Description": "Must not exceed 0.99"
                                              }
                                              """
                                             )!
                                     }
                                    )
                              , new KeyValuePair<DateTime, SubRestrictions>
                                    (new DateTime(1597, 2, 14)
                                   , new SubRestrictions
                                     {
                                         Enforcable = true
                                       , SubDescription = JsonSerializer.Deserialize<JsonObject>
                                             ("""
                                              { 
                                                "Name": "Vechicle Rating",
                                                "Description": "Must have autodriver assist"
                                              }
                                              """
                                             )!
                                     }
                                    )
                            ]
                        }
                      , new Restrictions
                        {
                            Name = "Experienced Driver Side Passenger"
                          , Type = new StringBuilder("Unassisted Driving")

                          , SubRestrictionsApplied = null!
                        }
                    ]
                })
            }
           ,
            {
                eidDiesel, new Lecturer("Vatican Library, Vactican City, Italy, Rome, 1")
                    {
                        EmployeeId     = eidDiesel
                      , DateOfBirth    = new DateTime(1967, 7, 18)
                      , FirstName      = "Wine"
                      , LastName       = new CharArrayStringBuilder("Diesel")
                      , UniquePersonId = 98120423m
                    }.AddStudentComplaint(sidPitt
                                        , new StringBuilder("Lecture rooms are so dark it's almost like the light hurts his eyes"))
                     .AddStudentComplaint(sidSimpson,
                                          new StringBuilder("Understanding of the Romantics is either poor or english is his not first language"))
            }
           ,
            {
                eidJoel
              , new TradesInstructor("Plumbing")
                {
                    EmployeeId     = eidJoel
                  , DateOfBirth    = new DateTime(1949, 5, 9)
                  , FirstName      = "William"
                  , LastName       = new MutableString("Joel")
                  , UniquePersonId = 2222111222
                  , SubjectMaxEntitledGradingLevel = new Dictionary<string, LicenseGrade>()
                    {
                        { "Hole Digging", LicenseGrade.Level5 }
                      , { "Main Water Water", LicenseGrade.Level2 }
                      , { "Hot Water System", LicenseGrade.Level4 }
                    }
                  , RecentAccreditations = [new DateTime(2025, 8, 1), null, new DateTime(2024, 6, 1)]
                }.AddLicense
                    (new HighPressureHydraulicsLicense("ChartingACourseForAtlantis")
                        {
                            LicenseNumber  = 9558271m
                          , ExpirationDate = new DateTime(1968, 11, 12)
                          , PhotoLocation
                                = new
                                    Uri("https://en.wikipedia.org/wiki/Piano_Man_(Billy_Joel_album)#/media/File:Billy_Joel_-_Piano_Man.jpg")
                        }.AddLicenseDetail(2, "Drippy drip drip drip")
                         .AddLicenseDetail(new DateTime(1973, 12, 2), new DirectoryInfo("c:\\"))
                         .AddLicenseDetail("Stranger", new DateOnly[]
                                               { new(1974, 1, 1), new(1975, 7, 7) })
                    )
            }
           ,
            {
                eidLennon
              , new Lecturer("1 Pearly Gates, Heaves Gate, New York")
                    {
                        EmployeeId     = eidLennon
                      , DateOfBirth    = new DateTime(1940, 12, 8)
                      , FirstName      = "Jimmy"
                      , LastName       = new CharArrayStringBuilder("Lennon")
                      , UniquePersonId = 84732738
                    }.AddStudentComplaint
                         (sidZoidberg
                        , new StringBuilder("Seems wasted half the time.  " +
                                            "Prefers to comment on society than sticking to the course material"))
                     .AddStudentComplaint
                         (sidConnery
                        , new StringBuilder("On behalf of decent men I think Professor Lennon harasses the female students"))
                     .AddStudentComplaint
                         (sidShatner
                        , new StringBuilder("Lacks volume when talking about thrusters, impulse, velocity and phases." +
                                            "  Very hard to understand him he seems to change topics randomly"))
                     .AddLicense(new MotorbikeLicense("1M4G1N3".ToCharArray())
                     {
                         ExpirationDate = new DateTime(1981, 3, 18)
                       , PhotoLocation = new Uri("https://en.wikipedia.org/wiki/John_Lennon#/media/File:John_Lennon,_1974_(restored_cropped).jpg")
                       , PowerToWeightLimit = (500, 204m)
                     })
            }
           ,
            {
                eidSchwarznegger
              , new Lecturer("I am a learning computer")
                    {
                        EmployeeId     = eidSchwarznegger
                      , DateOfBirth    = new DateTime(1947, 7, 30)
                      , FirstName      = "Arnie"
                      , LastName       = new MutableString("Schwarznegger")
                      , UniquePersonId = 98100283719m
                    }.AddStudentComplaint
                         (sidWilliams
                        , new StringBuilder("Guy doesn't take anything serious.  Always cracking Jokes, and putting on accents"))
                     .AddStudentComplaint
                         (sidPitt
                        , new StringBuilder("I can barely see the guy even in the front row, he is tiny.  " +
                                            "Sometimes he can't reach notes on the book shelf"))
                     .AddStudentComplaint
                         (sidConnery
                        , new StringBuilder("This my elective subject, He really knows his \"15th century\" early renaissance stuff, " +
                                            "however I would prefer something related to the course"))
            }
           ,
            {
                eidGroot
              , new Lecturer("Groot. Groooot. Groooooooot")
                    {
                        EmployeeId     = eidGroot
                      , DateOfBirth    = new DateTime(1960, 11, 1)
                      , FirstName      = "Groot"
                      , LastName       = null
                      , UniquePersonId = 0.8372831m
                    }.AddStudentComplaint
                         (sidCostanza
                        , new StringBuilder("I'm not sure I understand what Groot is saying.  Is he teaching it in greek"))
                     .AddStudentComplaint
                         (sidSimpson
                        , new StringBuilder("The guy is such a great orator.  I'm learning 10 new works every class"))
                     .AddStudentComplaint
                         (sidZoidberg
                        , new StringBuilder("Is it mean or can this guy only say the same thing over and over again.  " +
                                            "Everyone else seems to understand what he is saying but to me it's all greek"))
                     .AddLicense(new AutomobileLicense
                     {
                         LicenseNumber       = 2093819238
                       , ExpirationDate      = new DateTime(2034, 8, 12)
                       , CurrentRestrictions = null
                       , Citations =
                         [
                             new KeyValuePair<DateTime, string>
                                 (
                                  new DateTime(2007, 6, 2)
                                , "Practicing medicine whilst driving"
                                 )
                           , new KeyValuePair<DateTime, string>
                                 (
                                  new DateTime(2008, 1, 2)
                                , "Driving whilst covered in shrimp"
                                 )
                            ,
                         ]
                     })
            }
        };
        Faculties =
        [
            new KeyValuePair<FacultyType, Faculty>
                (
                 FacultyType.ElectricianSchool
               , new TradeSkills(new MutableString("SparkiesUnited"), FacultyType.ElectricianSchool)
                 {
                     BoardOfFaculty =
                     [
                         CourseDeliverers[eidShakespeare], CourseDeliverers[eidJoel]
                     ]
                 }
                )
          , new KeyValuePair<FacultyType, Faculty>
                (
                 FacultyType.Literature
               , new ArtsFaculty(new MutableString("Literature"), FacultyType.Literature)
                 {
                     DeanOfFaculty = CourseDeliverers[eidDiesel]
                 }
                )
          , new KeyValuePair<FacultyType, Faculty>
                (
                 FacultyType.PlumbingSchool
               , new TradeSkills(new MutableString("DrippiesUnited"), FacultyType.PlumbingSchool)
                 {
                     BoardOfFaculty =
                     [
                         CourseDeliverers[eidJoel], CourseDeliverers[eidJoel]
                     ]
                 }
                )
          , new KeyValuePair<FacultyType, Faculty>
                (
                 FacultyType.AeronauticalEngineering
               , new Engineering(new MutableString("AeronauticalEngineering"), FacultyType.AeronauticalEngineering)
                 {
                     DeanOfFaculty  = (Lecturer)CourseDeliverers[eidLennon]
                   , MajorOfFaculty = "Aeronautics/Mechanical".ToArray()
                 }
                )
          , new KeyValuePair<FacultyType, Faculty>
                (
                 FacultyType.FineArts
               , new ArtsFaculty(new MutableString("Fine Arts"), FacultyType.FineArts)
                 {
                     DeanOfFaculty = CourseDeliverers[eidSchwarznegger]
                 }
                )
          , new KeyValuePair<FacultyType, Faculty>
                (
                 FacultyType.AncientGreekPhilosophy
               , new ArtsFaculty(new MutableString("Ancient Greek Arts"), FacultyType.AncientGreekPhilosophy)
                 {
                     DeanOfFaculty = CourseDeliverers[eidGroot]
                 }
                )
        ];
        Subjects = new Dictionary<string, CourseSubject>()
        {
            {
                "LA101", new ArtsSubject("19th Century Romantics")
                {
                    ManagingArtsFaculty = (ArtsFaculty)Faculties.Single(kvp => kvp.Key == FacultyType.Literature).Value
                  , SubjectOwner        = (Lecturer)CourseDeliverers[eidDiesel]
                }
            }
           ,
            {
                "FA102", new ArtsSubject("15-16th Century Renaissance")
                {
                    ManagingArtsFaculty = (ArtsFaculty)Faculties.Single(kvp => kvp.Key == FacultyType.FineArts).Value
                  , SubjectOwner        = (Lecturer)CourseDeliverers[eidSchwarznegger]
                }
            }
           ,
            {
                "AH103", new ArtsSubject("Modern Greek Influences")
                {
                    ManagingArtsFaculty = (ArtsFaculty)Faculties.Single(kvp => kvp.Key == FacultyType.AncientGreekPhilosophy).Value
                  , SubjectOwner        = (Lecturer)CourseDeliverers[eidGroot]
                }
            }
           ,
            {
                "LA111", new ArtsSubject("20th Century Works")
                {
                    ManagingArtsFaculty = (ArtsFaculty)Faculties.Single(kvp => kvp.Key == FacultyType.Literature).Value
                  , SubjectOwner        = (Lecturer)CourseDeliverers[eidDiesel]
                }
            }
           ,
            {
                "FA111", new ArtsSubject("Picasso")
                {
                    ManagingArtsFaculty = (ArtsFaculty)Faculties.Single(kvp => kvp.Key == FacultyType.FineArts).Value
                  , SubjectOwner        = (Lecturer)CourseDeliverers[eidSchwarznegger]
                }
            }
           ,
            {
                "AH111", new ArtsSubject("Age of Heros")
                {
                    ManagingArtsFaculty = (ArtsFaculty)Faculties.Single(kvp => kvp.Key == FacultyType.AncientGreekPhilosophy).Value
                  , SubjectOwner        = (Lecturer)CourseDeliverers[eidGroot]
                }
            }
           ,
            {
                "EL101", new TradesSubject("Electronics Fundamentals")
                {
                    RequiredAttendeeLicense = null
                  , RequiredTeacherLicense  = new HighVoltageElectriciansLicense("Level 2 - Polls and Wires")

                  , RecentGrades = new LinkedList<double?>(TestMaps.IntToDoubleMap.Select(kvp => (double?)kvp.Value))
                  , SubjectOwner = (AccredittedInstructor)CourseDeliverers[eidShakespeare]
                }
            }
           ,
            {
                "MA102", new EngineeringSubject("Calculus for Fluids")
                {
                    RecentGrades
                        = new LinkedList<double?>(TestMaps.IntToDoubleMap.Select(kvp => (double?)kvp.Value / 10))
                  , CourseCoordinator = (Lecturer)CourseDeliverers[eidLennon]
                }
            }
           ,
            {
                "PB101", new TradesSubject("Tools of the Trade")
                {
                    RequiredAttendeeLicense = null
                  , RequiredTeacherLicense  = new HighPressureHydraulicsLicense("Level 1 - Domestic Water supply")

                  , RecentGrades = new LinkedList<double?>(TestMaps.IntToDoubleMap.Select(kvp => (double?)kvp.Value))
                  , SubjectOwner = (AccredittedInstructor)CourseDeliverers[eidJoel]
                }
            }
           ,
            {
                "EL102", new TradesSubject("Power Safety")
                {
                    RequiredAttendeeLicense = null
                  , RequiredTeacherLicense  = new HighVoltageElectriciansLicense("Level 1 - Polls and Wires")

                  , RecentGrades = new LinkedList<double?>(TestMaps.IntToDoubleMap.Select(kvp => (double?)kvp.Value))
                  , SubjectOwner = (AccredittedInstructor)CourseDeliverers[eidShakespeare]
                }
            }
           ,
            {
                "MA103", new EngineeringSubject("Sochastic Processes")
                {
                    RecentGrades
                        = new LinkedList<double?>(TestMaps.IntToDoubleMap.Select(kvp => (double?)kvp.Value / 100))
                  , CourseCoordinator = (Lecturer)CourseDeliverers[eidLennon]
                }
            }
           ,
            {
                "PB102", new TradesSubject("Digging a hole")
                {
                    RequiredAttendeeLicense = null
                  , RequiredTeacherLicense  = new HighPressureHydraulicsLicense("Level 1 - Urban street plans")

                  , RecentGrades
                        = new LinkedList<double?>(TestMaps.IntToDoubleMap.Select(kvp => (double?)kvp.Value * 67))
                  , SubjectOwner = (AccredittedInstructor)CourseDeliverers[eidJoel]
                }
            }
        };

        CourseDeliverers[eidDiesel].CurrentCourses.Add("LA101", Subjects["LA101"]);
        CourseDeliverers[eidDiesel].CurrentCourses.Add("LA111", Subjects["LA111"]);
        CourseDeliverers[eidSchwarznegger].CurrentCourses.Add("FA102", Subjects["FA102"]);
        CourseDeliverers[eidSchwarznegger].CurrentCourses.Add("FA111", Subjects["FA111"]);
        CourseDeliverers[eidGroot].CurrentCourses.Add("AH103", Subjects["AH103"]);
        CourseDeliverers[eidGroot].CurrentCourses.Add("AH111", Subjects["AH111"]);
        CourseDeliverers[eidShakespeare].CurrentCourses.Add("EL101", Subjects["EL101"]);
        CourseDeliverers[eidShakespeare].CurrentCourses.Add("EL102", Subjects["EL102"]);
        CourseDeliverers[eidLennon].CurrentCourses.Add("MA102", Subjects["MA102"]);
        CourseDeliverers[eidLennon].CurrentCourses.Add("MA103", Subjects["MA103"]);
        CourseDeliverers[eidJoel].CurrentCourses.Add("PB101", Subjects["PB101"]);
        CourseDeliverers[eidJoel].CurrentCourses.Add("PB102", Subjects["PB102"]);

        ((EngineeringSubject)Subjects["MA103"]).RequiredPrerequisiteSubjects!.Add((EngineeringSubject)Subjects["MA102"]);

        Students = new()
        {
            {
                sidShatner, new Student
                {
                    DateOfBirth    = new DateTime(1931, 3, 22)
                  , StudentNumber  = sidShatner
                  , FirstName      = "Bill"
                  , LastName       = new CharArrayStringBuilder("Shatner")
                  , UniquePersonId = 5532930123
                  , Enrollments =
                        new Dictionary<string, CourseSubject>
                        {
                            { "MA102", Subjects["MA102"] }, { "MA103", Subjects["MA103"] }, { "LA101", Subjects["LA101"] }
                          , { "FA111", Subjects["FA111"] },
                        }
                }
            }
           ,
            {
                sidWilliams, new Student
                {
                    DateOfBirth    = new DateTime(1951, 7, 21)
                  , StudentNumber  = sidWilliams
                  , FirstName      = "Robyn"
                  , LastName       = new CharArrayStringBuilder("Williams")
                  , UniquePersonId = 5532930123
                  , Enrollments =
                        new Dictionary<string, CourseSubject>
                        {
                            { "FA102", Subjects["FA102"] }, { "FA111", Subjects["FA111"] }, { "PB101", Subjects["PB101"] }
                          , { "PB102", Subjects["PB102"] },
                        }
                }
            }
           ,
            {
                sidPitt, new Student
                {
                    DateOfBirth    = new DateTime(1963, 12, 18)
                  , StudentNumber  = sidPitt
                  , FirstName      = "Bradley"
                  , LastName       = new CharArrayStringBuilder("Pitt")
                  , UniquePersonId = 3498205
                  , Enrollments =
                        new Dictionary<string, CourseSubject>
                        {
                            { "EL101", Subjects["EL101"] }, { "EL102", Subjects["EL102"] }, { "LA101", Subjects["LA101"] }
                          , { "LA111", Subjects["LA111"] },
                        }
                }
            }
           ,
            {
                sidSimpson, new Student
                {
                    DateOfBirth    = new DateTime(1983, 3, 3)
                  , StudentNumber  = sidSimpson
                  , FirstName      = "Lisa"
                  , LastName       = new CharArrayStringBuilder("Simpson")
                  , UniquePersonId = 38477892
                  , Enrollments =
                        new Dictionary<string, CourseSubject>
                        {
                            { "EL101", Subjects["EL101"] }, { "EL102", Subjects["EL102"] }, { "AH111", Subjects["AH111"] }
                          , { "AH103", Subjects["AH103"] },
                        }
                }
            }
           ,
            {
                sidCostanza, new Student
                {
                    DateOfBirth    = new DateTime(1958, 4, 17)
                  , StudentNumber  = sidCostanza
                  , FirstName      = "George"
                  , LastName       = new MutableString("Costanza")
                  , UniquePersonId = 69029381
                  , Enrollments =
                        new Dictionary<string, CourseSubject>
                        {
                            { "FA102", Subjects["FA102"] }, { "FA111", Subjects["FA111"] }, { "PB101", Subjects["PB101"] }
                          , { "PB102", Subjects["PB102"] },
                        }
                }
            }
           ,
            {
                sidZoidberg, new Student
                {
                    DateOfBirth    = new DateTime(2198, 10, 1)
                  , StudentNumber  = sidZoidberg
                  , FirstName      = "Zoidberge"
                  , UniquePersonId = 9999999992837
                  , Enrollments =
                        new Dictionary<string, CourseSubject>
                        {
                            { "PB101", Subjects["PB101"] }, { "PB102", Subjects["PB102"] }, { "EL102", Subjects["EL102"] }
                          , { "EL101", Subjects["EL101"] },
                        }
                }
            }
        };

        ((TradeSkills)Faculties.Single(kvp => kvp.Key == FacultyType.ElectricianSchool).Value).CurrentStudents
                                                                                              .AddOrUpdate(3498205, (Student)Students[sidPitt]
                                                                                             , (_, _) => (Student)Students[sidPitt]);

        ((TradeSkills)Faculties.Single(kvp => kvp.Key == FacultyType.ElectricianSchool).Value).CurrentStudents
                                                                                              .AddOrUpdate(38477892, (Student)Students[sidSimpson]
                                                                                             , (_, _) => (Student)Students[sidSimpson]);

        ((TradeSkills)Faculties.Single(kvp => kvp.Key == FacultyType.ElectricianSchool).Value).CurrentStudents
                                                                                              .AddOrUpdate(9999999992837
                                                                                             , (Student)Students[sidZoidberg]
                                                                                             , (_, _) => (Student)Students[sidZoidberg]);
    }
}

public class Person : IStringBearer
{
    public string FirstName { get; set; } = null!;

    public ICharSequence? LastName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime DateOfBirth { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<License>? Licenses { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(FirstName), FirstName)
            .Field.WhenNonDefaultAdd(nameof(DateOfBirth), DateOfBirth)
            .Field.WhenNonNullOrDefaultAddCharSeq(nameof(LastName), LastName)
            .CollectionField.WhenNonNullRevealAll(nameof(Licenses), Licenses)
            .Complete();

    public override string ToString() =>
        $"{nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, " +
        $"{nameof(DateOfBirth)}: {DateOfBirth}, {nameof(Licenses)}: {Licenses}";
}

public abstract class EducationAttendee : Person
{
    public decimal UniquePersonId { get; set; }
}

public class Student : EducationAttendee
{
    public ICharSequence StudentNumber { get; set; } = new MutableString();

    public Dictionary<string, CourseSubject> Enrollments { get; set; } = new();

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAddCharSeq(nameof(StudentNumber), StudentNumber)
            .KeyedCollectionField.AlwaysAddAll(nameof(Enrollments), Enrollments)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() => $"{base.ToString()}, {nameof(StudentNumber)}: {StudentNumber}, {nameof(Enrollments)}: {Enrollments}";
}

public class CourseDeliverer : EducationAttendee
{
    public ICharSequence EmployeeId { get; set; } = new CharArrayStringBuilder();

    public Dictionary<string, CourseSubject> CurrentCourses { get; set; } = new();

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .CollectionField.AlwaysAddAllEnumerate(nameof(EmployeeId), EmployeeId)
            .KeyedCollectionField.AlwaysAddAll(nameof(CurrentCourses), CurrentCourses)
            .AddBaseStyledToStringFields(this);

    public override string ToString() =>
        $"{base.ToString()}, {nameof(EmployeeId)}: {EmployeeId}, {nameof(CurrentCourses)}.Count: {CurrentCourses.Count}";
}

public class AccredittedInstructor : CourseDeliverer
{
    public Dictionary<string, LicenseGrade> SubjectMaxEntitledGradingLevel { get; set; } = new();

    public AccredittedInstructor AddLicense(License toAdd)
    {
        Licenses ??= new List<License>();

        Licenses.Add(toAdd);

        return this;
    }


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .KeyedCollectionField.AlwaysAddAll(nameof(SubjectMaxEntitledGradingLevel), SubjectMaxEntitledGradingLevel)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() => $"{base.ToString()}, {nameof(SubjectMaxEntitledGradingLevel)}: {SubjectMaxEntitledGradingLevel}";
}

public class Lecturer(string officeAddress) : CourseDeliverer
{
    public string OfficeAddress { get; } = officeAddress;

    public Dictionary<string, CourseSubject> TeachingSubjects { get; } = null!;


    public Lecturer AddLicense(License toAdd)
    {
        Licenses ??= new List<License>();

        Licenses.Add(toAdd);

        return this;
    }

    public Lecturer AddStudentComplaint(ICharSequence studentNumber, StringBuilder complaint)
    {
        Complaints.Add(studentNumber, complaint);

        return this;
    }

    public ConcurrentMap<ICharSequence, StringBuilder> Complaints { get; private set; } = new();

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(OfficeAddress), OfficeAddress)
            .KeyedCollectionField.AlwaysAddAll(nameof(TeachingSubjects), TeachingSubjects)
            .AddBaseStyledToStringFields(this)
            .Complete();


    public override string ToString() =>
        $"{base.ToString()}, {nameof(OfficeAddress)}: {OfficeAddress}, " +
        $"{nameof(TeachingSubjects)}: " +
        string.Join(", ", TeachingSubjects.Select(cs => cs.Value.SubjectName)) +
        $", {nameof(Complaints)}: {Complaints}";
}

public class TradesInstructor(string tradeSkill) : AccredittedInstructor
{
    public List<DateTime?> RecentAccreditations { get; set; } = new();

    public string TradeSkill { get; set; } = tradeSkill;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(TradeSkill), TradeSkill)
            .CollectionField.AlwaysAddAll(nameof(RecentAccreditations), RecentAccreditations)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() =>
        $"{base.ToString()}, {nameof(RecentAccreditations)}: {RecentAccreditations}, {nameof(TradeSkill)}: {TradeSkill}";
}

[JsonDerivedType(typeof(ArtsSubject))]
[JsonDerivedType(typeof(EngineeringSubject))]
[JsonDerivedType(typeof(TradesSubject))]
public record CourseSubject(string SubjectName) : IStringBearer
{
    public LinkedList<double?> RecentGrades { get; set; } = new();

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(SubjectName), SubjectName)
            .CollectionField.AlwaysAddAllEnumerate(nameof(RecentGrades), RecentGrades).Complete();

    public override string ToString() => $"{nameof(RecentGrades)}: {RecentGrades}, {nameof(SubjectName)}: {SubjectName}";
}

public record ArtsSubject(string SubjectName) : CourseSubject(SubjectName)
{
    public ArtsFaculty ManagingArtsFaculty { get; set; } = null!;

    public Lecturer SubjectOwner { get; set; } = null!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysReveal(nameof(ManagingArtsFaculty), ManagingArtsFaculty)
            .Field.AlwaysReveal(nameof(SubjectOwner), SubjectOwner)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() =>
        $"{base.ToString()}, {nameof(ManagingArtsFaculty)}.Name: {ManagingArtsFaculty.Name.ToString()}, {nameof(SubjectOwner)}: {SubjectOwner}";
}

public record EngineeringSubject(string SubjectName) : CourseSubject(SubjectName)
{
    public Lecturer CourseCoordinator { get; set; } = null!;
    public List<EngineeringSubject>? RequiredPrerequisiteSubjects { get; } = new();

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .CollectionField.WhenPopulatedRevealAll(nameof(RequiredPrerequisiteSubjects), RequiredPrerequisiteSubjects)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() =>
        $"{base.ToString()}, {nameof(CourseCoordinator)}: {CourseCoordinator}" +
        $", {nameof(RequiredPrerequisiteSubjects)}.Name: "
      + string.Join(", ", (RequiredPrerequisiteSubjects ?? []).Select(cs => cs.SubjectName));
}

public record TradesSubject(string SubjectName) : CourseSubject(SubjectName)
{
    public License? RequiredAttendeeLicense { get; set; }

    public License RequiredTeacherLicense { get; set; } = null!;

    public AccredittedInstructor SubjectOwner { get; set; } = null!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysReveal(nameof(RequiredAttendeeLicense), RequiredAttendeeLicense)
            .Field.AlwaysReveal(nameof(RequiredTeacherLicense), RequiredTeacherLicense)
            .Field.AlwaysReveal(nameof(SubjectOwner), SubjectOwner)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() =>
        $"{base.ToString()}, {nameof(RequiredAttendeeLicense)}: {RequiredAttendeeLicense}" +
        $", {nameof(RequiredTeacherLicense)}: {RequiredTeacherLicense}" +
        $", {nameof(SubjectOwner)}.FirstName: {SubjectOwner.FirstName}";
}

public enum FacultyType
{
    Literature              = 1
  , FarmSkills              = 2
  , FineArts                = 3
  , FittingAndTurningSchool = 4
  , AncientGreekPhilosophy  = 5
  , PlumbingSchool          = 6
  , AeronauticalEngineering = 7
  , ElectricianSchool       = 8
};

public abstract class Faculty : IStringBearer
{
    protected Faculty()
    {
        // not setting Faculty type
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    protected Faculty(ICharSequence facultyName, FacultyType facultyType)
    {
        Name        = facultyName;
        FacultyType = facultyType;
    }

    public ICharSequence Name { get; set; } = null!;

    public FacultyType FacultyType { get; set; }

    public abstract StateExtractStringRange RevealState(ITheOneString tos);
}

public class ArtsFaculty : Faculty
{
    public ArtsFaculty(ICharSequence facultyName, FacultyType facultyType) : base(facultyName, facultyType) { }

    public Person DeanOfFaculty { get; set; } = null!;

    public ConcurrentDictionary<ICharSequence, Student> CurrentStudents = new();

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(FacultyType), FacultyType)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() =>
        $"{nameof(CurrentStudents)}.FirstNames: " + string.Join(", ", CurrentStudents.Values.Select(s => s.FirstName)) +
        $", {nameof(DeanOfFaculty)}.FirstName: {DeanOfFaculty.FirstName}";
}

public class TradeSkills : Faculty
{
    public TradeSkills(ICharSequence facultyName, FacultyType facultyType) : base(facultyName, facultyType) { }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<CourseDeliverer> BoardOfFaculty { get; set; } = new();

    public readonly ConcurrentDictionary<decimal, Student> CurrentStudents = new();

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(FacultyType), FacultyType)
            .CollectionField.WhenPopulatedRevealAll(nameof(BoardOfFaculty), BoardOfFaculty)
            .KeyedCollectionField.AlwaysAddAll(nameof(CurrentStudents), CurrentStudents)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() => $"{nameof(CurrentStudents)}: {CurrentStudents}, {nameof(BoardOfFaculty)}: {BoardOfFaculty}";
}

public class Engineering : Faculty
{
    public Engineering(ICharSequence facultyName, FacultyType facultyType) : base(facultyName, facultyType) { }

    public char[] MajorOfFaculty { get; set; } = null!;

    public Lecturer DeanOfFaculty { get; set; } = null!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(FacultyType), FacultyType)
            .Field.AlwaysAdd(nameof(MajorOfFaculty), MajorOfFaculty)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() => $"{nameof(MajorOfFaculty)}: {MajorOfFaculty}, {nameof(DeanOfFaculty)}.FirstName: {DeanOfFaculty.FirstName}";
}

[JsonDerivedType(typeof(HighVoltageElectriciansLicense))]
[JsonDerivedType(typeof(HighPressureHydraulicsLicense))]
[JsonDerivedType(typeof(MotorbikeLicense))]
[JsonDerivedType(typeof(AutomobileLicense))]
public class License : IStringBearer
{
    public decimal LicenseNumber { get; set; }

    public byte[] LicenseChipData { get; set; } = [];

    public DateTime ExpirationDate { get; set; }

    public Uri PhotoLocation { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(LicenseNumber), LicenseNumber)
            .CollectionField.AlwaysAddAll(nameof(LicenseChipData), LicenseChipData)
            .Field.WhenNonDefaultAdd(nameof(ExpirationDate), ExpirationDate)
            .Field.AlwaysAdd(nameof(PhotoLocation), PhotoLocation);

    public override string ToString() =>
        $"{nameof(LicenseNumber)}: {LicenseNumber}, {nameof(LicenseChipData)}: {LicenseChipData}" +
        $", {nameof(ExpirationDate)}: {ExpirationDate}, {nameof(PhotoLocation)}: {PhotoLocation}";
}

public enum LicenseGrade
{
    Level1 = 1
  , Level2 = 2
  , Level3 = 3
  , Level4 = 4
  , Level5 = 5
}

public class HighVoltageElectriciansLicense : License
{
    public HighVoltageElectriciansLicense()
    {
        LicenseChipData = [];
        Issued          = new DateTime();
    }


    public HighVoltageElectriciansLicense(string chipData)
    {
        LicenseChipData = Encoding.UTF8.GetBytes(chipData);
        Issued          = new DateTime();
    }

    public DateTime Issued { get; set; }

    public LicenseGrade GradeOfAccess { get; set; }

    public AccredittedInstructor AccreditingBody { get; set; } = null!;

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.WhenNonDefaultAdd(nameof(Issued), Issued)
            .Field.AlwaysAdd(nameof(GradeOfAccess), GradeOfAccess)
            .AddBaseStyledToStringFields(this);

    public override string ToString() =>
        $"{base.ToString()}, {nameof(Issued)}: {Issued}, {nameof(GradeOfAccess)}: {GradeOfAccess}, " +
        $"{nameof(AccreditingBody)}: {AccreditingBody}";
}

public class HighPressureHydraulicsLicense : License
{
    public HighPressureHydraulicsLicense()
    {
        LicenseChipData = [];
    }

    public HighPressureHydraulicsLicense(string chipData)
    {
        LicenseChipData = Encoding.UTF8.GetBytes(chipData);
    }

    public HighPressureHydraulicsLicense AddLicenseDetail(object licenseKeyDetail, object licenseDetail)
    {
        LicenseDetails ??= new();
        LicenseDetails.Add(licenseKeyDetail, licenseDetail);

        return this;
    }

    private Dictionary<object, object?>? LicenseDetails { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .KeyedCollectionField.WhenNonNullAddAll(nameof(LicenseDetails), LicenseDetails)
            .AddBaseStyledToStringFields(this)
            .Complete();

    public override string ToString() => $"{base.ToString()}, {nameof(LicenseDetails)}: {LicenseDetails}";
}

public class MotorbikeLicense : License
{
    public MotorbikeLicense()
    {
        LicenseChipData = null!;
        LicensePlate    = null!;
    }


    public MotorbikeLicense(char[] licensePlate)
    {
        LicenseChipData = [];
        LicensePlate    = licensePlate;
    }


    public (int, decimal) PowerToWeightLimit { get; set; }

    public char[] LicensePlate { get; set; }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAddObject(nameof(PowerToWeightLimit), PowerToWeightLimit)
            .Field.AlwaysAdd(nameof(LicensePlate), LicensePlate)
            .AddBaseStyledToStringFields(this);

    public override string ToString() =>
        $"{base.ToString()}, {nameof(PowerToWeightLimit)}: {PowerToWeightLimit}, {nameof(LicensePlate)}: {LicensePlate}";
}

public class AutomobileLicense : License
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Manual { get; set; }

    public Restrictions[]? CurrentRestrictions { get; set; } = [];

    public List<KeyValuePair<DateTime, string>> Citations { get; set; } = new();

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.WhenNonDefaultAdd(nameof(Manual), Manual)
            .CollectionField.AlwaysRevealAll(nameof(CurrentRestrictions), CurrentRestrictions, Restrictions.Styler)
            .KeyedCollectionField.AlwaysAddAll(nameof(Citations), Citations, null, "givenOn_{0:yyyyMMdd}")
            .AddBaseStyledToStringFields(this);

    public override string ToString() =>
        $"{base.ToString()}, {nameof(Manual)}: {Manual}, {nameof(CurrentRestrictions)}: {CurrentRestrictions}, {nameof(Citations)}: {Citations}";
}

public struct Restrictions(TimeSpan length)
{
    [JsonInclude] private TimeSpan lengthRemaining = length;
    public string Name { get; set; } = "";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public StringBuilder Type { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public KeyValuePair<DateTime, SubRestrictions>[] SubRestrictionsApplied { get; set; } = null!;

    public static PalantírReveal<Restrictions> Styler { get; } =
        (restriction, stsa) =>
            stsa.StartComplexType(restriction)
                .Field.AlwaysAdd(nameof(lengthRemaining), restriction.lengthRemaining)
                .Field.AlwaysAdd(nameof(Name), restriction.Name)
                .Field.WhenNonNullOrDefaultAdd(nameof(Type), restriction.Type)
                .KeyedCollectionField
                .WhenNonNullAddAll(nameof(SubRestrictionsApplied), restriction.SubRestrictionsApplied, SubRestrictions.Styler)
                .Complete();

    public override string ToString() =>
        $"{nameof(lengthRemaining)}: {lengthRemaining}, {nameof(Name)}: {Name}, " +
        $"{nameof(Type)}: {Type}, {nameof(SubRestrictionsApplied)}: {SubRestrictionsApplied}";
}

public struct SubRestrictions()
{
    public JsonObject SubDescription { get; set; } = null!;

    public bool Enforcable { get; set; }

    public static PalantírReveal<SubRestrictions> Styler { get; } =
        (subRest, stsa) =>
            stsa.StartComplexType(subRest)
                .Field.AlwaysAddObject(nameof(SubDescription), subRest.SubDescription)
                .Field.AlwaysAdd(nameof(Enforcable), subRest.Enforcable)
                .Complete();

    public override string ToString() => $"{nameof(SubDescription)}: {SubDescription}, {nameof(Enforcable)}: {Enforcable}";
}
