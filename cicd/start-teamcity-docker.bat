@echo off
SET "checkStarted= "
FOR /F "tokens=* USEBACKQ" %%F IN (`docker ps -a ^| findstr teamcity-server-instance`) DO (
SET checkStarted=%%F
)
ECHO Checking for existing returned "%checkStarted%"

IF ["%checkStarted%"] == [" "] GOTO CREATE_NEW_CONTAINER

IF NOT ["x%checkStarted:Exited=%"]==["x%checkStarted%"] GOTO RESTART_EXISTING_CONTAINER
ECHO Teamcity Server Container is already running
EXIT /B

:RESTART_EXISTING_CONTAINER
ECHO starting existing container 'teamcity-server-instance'
docker start teamcity-server-instance
ECHO Started existing teamcity server container
EXIT /B

:CREATE_NEW_CONTAINER
ECHO launching new container 'teamcity-server-instance'
docker run -it -d --name teamcity-server-instance -v %cd%\teamcity\config:/data/teamcity_server/datadir -v %cd%\teamcity\logs:/opt/teamcity/logs -p 8111:8111 jetbrains/teamcity-server 
ECHO Started new teamcity server container
EXIT /B
