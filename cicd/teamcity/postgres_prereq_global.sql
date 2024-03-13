--
-- PostgreSQL database cluster dump
--

-- Started on 2024-03-13 11:40:18

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE postgres;
-- postgres pwd - shwaindog
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:uw3WlldofZyD8pSdbHeaYg==$cf9mCxqpEyL42NPKUO+xSCiGMbn160g8L3df6LLw2RU=:K8udDysHVH9tjuuH6W1tR20IHsMgYfItBd7e03BXu9Y=';
CREATE ROLE teamcity;
-- teamcity pwd - teamcityFortitude
ALTER ROLE teamcity WITH NOSUPERUSER INHERIT CREATEROLE NOCREATEDB LOGIN REPLICATION NOBYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:Bzyc1OTNCUEHnUkl29i8Pg==$FTvvXoTomm8woyWz3zREDTgCc+4KBfPXBXA2bPQH1cw=:UngrizJhljLaDCLVX+h9iejo6llckAEDHqeqEISAtyU=';

--
-- User Configurations
--


--
-- Role memberships
--

GRANT pg_checkpoint TO teamcity WITH INHERIT TRUE GRANTED BY postgres;
GRANT pg_create_subscription TO teamcity WITH INHERIT TRUE GRANTED BY postgres;
GRANT pg_read_all_data TO teamcity WITH INHERIT TRUE GRANTED BY postgres;
GRANT pg_signal_backend TO teamcity WITH INHERIT TRUE GRANTED BY postgres;
GRANT pg_stat_scan_tables TO teamcity WITH INHERIT TRUE GRANTED BY postgres;
GRANT pg_write_all_data TO teamcity WITH INHERIT TRUE GRANTED BY postgres;


--
-- Tablespaces
--

CREATE TABLESPACE teamcity OWNER postgres LOCATION E'C:\\data\\teamcity\\teamcity_tablespace';


CREATE DATABASE teamcity
    WITH
    OWNER = teamcity
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = teamcity
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;
-- Completed on 2024-03-13 11:40:18

--
-- PostgreSQL database cluster dump complete
--

