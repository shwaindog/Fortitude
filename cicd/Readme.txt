The cicd configuration contain in this directory includes the teamcity /data/teamcity_server/datadir minus the ssh_key to access the SVN repo and the contents of
/data/teamcity_server/datadir/config/system/caches/**/*

Teamcity requires a database and this instance is currently configured for

Database type: PostgreSQL
jdbc:postgresql://host.docker.internal:8101/teamcity

where host.docker.internal points to the docker hostmachine.

Teamcity can install the database if given an endpoint and user with valid permissions. 


Windows

docker run -it -d --name teamcity-server-instance -u root -v %cd%\teamcity\config:/data/teamcity_server/datadir -v %cd%\teamcity\logs:/opt/teamcity/logs -p 8111:8111 jetbrains/teamcity-server


Linux

docker run -it -d --name teamcity-server-instance -u root -v $(pwd)/teamcity/config:/data/teamcity_server/datadir -v $(pwd)/teamcity/logs:/opt/teamcity/logs -p 8111:8111 jetbrains/teamcity-server


Launch Teamcity website

http://localhost:8111

