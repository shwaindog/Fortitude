--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1
-- Dumped by pg_dump version 16.0

-- Started on 2024-03-13 11:34:32


SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 361 (class 1259 OID 17411)
-- Name: action_history; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.action_history (
    object_id character varying(80),
    comment_id bigint,
    action integer,
    additional_data character varying(80)
);


ALTER TABLE public.action_history OWNER TO teamcity;

--
-- TOC entry 253 (class 1259 OID 16663)
-- Name: agent; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.agent (
    id integer NOT NULL,
    name character varying(256) NOT NULL,
    host_addr character varying(80) NOT NULL,
    port integer NOT NULL,
    agent_type_id integer NOT NULL,
    status integer,
    authorized integer,
    registered integer,
    registration_timestamp bigint,
    last_binding_timestamp bigint,
    unregistered_reason character varying(256),
    authorization_token character varying(32),
    status_to_restore integer,
    status_restoring_timestamp bigint,
    version character varying(80),
    plugins_version character varying(80)
);


ALTER TABLE public.agent OWNER TO teamcity;

--
-- TOC entry 247 (class 1259 OID 16628)
-- Name: agent_pool; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.agent_pool (
    agent_pool_id integer NOT NULL,
    agent_pool_name character varying(191),
    min_agents integer,
    max_agents integer,
    owner_project_id character varying(80)
);


ALTER TABLE public.agent_pool OWNER TO teamcity;

--
-- TOC entry 285 (class 1259 OID 16883)
-- Name: agent_pool_project; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.agent_pool_project (
    agent_pool_id integer NOT NULL,
    project_int_id character varying(80) NOT NULL
);


ALTER TABLE public.agent_pool_project OWNER TO teamcity;

--
-- TOC entry 248 (class 1259 OID 16633)
-- Name: agent_type; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.agent_type (
    agent_type_id integer NOT NULL,
    agent_pool_id integer NOT NULL,
    cloud_code character varying(6) NOT NULL,
    profile_id character varying(30) NOT NULL,
    image_id character varying(60) NOT NULL,
    policy integer NOT NULL
);


ALTER TABLE public.agent_type OWNER TO teamcity;

--
-- TOC entry 280 (class 1259 OID 16851)
-- Name: agent_type_bt_access; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.agent_type_bt_access (
    agent_type_id integer NOT NULL,
    build_type_id character varying(80) NOT NULL
);


ALTER TABLE public.agent_type_bt_access OWNER TO teamcity;

--
-- TOC entry 249 (class 1259 OID 16641)
-- Name: agent_type_info; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.agent_type_info (
    agent_type_id integer NOT NULL,
    os_name character varying(60) NOT NULL,
    cpu_rank integer,
    created_timestamp timestamp without time zone,
    modified_timestamp timestamp without time zone
);


ALTER TABLE public.agent_type_info OWNER TO teamcity;

--
-- TOC entry 252 (class 1259 OID 16656)
-- Name: agent_type_param; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.agent_type_param (
    agent_type_id integer NOT NULL,
    param_kind character(1) NOT NULL,
    param_name character varying(160) NOT NULL,
    param_value character varying(2000)
);


ALTER TABLE public.agent_type_param OWNER TO teamcity;

--
-- TOC entry 250 (class 1259 OID 16646)
-- Name: agent_type_runner; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.agent_type_runner (
    agent_type_id integer NOT NULL,
    runner character varying(250) NOT NULL
);


ALTER TABLE public.agent_type_runner OWNER TO teamcity;

--
-- TOC entry 251 (class 1259 OID 16651)
-- Name: agent_type_vcs; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.agent_type_vcs (
    agent_type_id integer NOT NULL,
    vcs character varying(250) NOT NULL
);


ALTER TABLE public.agent_type_vcs OWNER TO teamcity;

--
-- TOC entry 362 (class 1259 OID 17417)
-- Name: audit_additional_object; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.audit_additional_object (
    comment_id bigint,
    object_index integer,
    object_id character varying(80),
    object_name character varying(2500)
);


ALTER TABLE public.audit_additional_object OWNER TO teamcity;

--
-- TOC entry 237 (class 1259 OID 16566)
-- Name: backup_builds; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.backup_builds (
    build_id bigint NOT NULL
);


ALTER TABLE public.backup_builds OWNER TO teamcity;

--
-- TOC entry 236 (class 1259 OID 16558)
-- Name: backup_info; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.backup_info (
    mproc_id integer NOT NULL,
    file_name character varying(1000),
    file_size bigint,
    started timestamp without time zone NOT NULL,
    finished timestamp without time zone,
    status character(1)
);


ALTER TABLE public.backup_info OWNER TO teamcity;

--
-- TOC entry 311 (class 1259 OID 17093)
-- Name: build_artifact_dependency; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_artifact_dependency (
    artif_dep_id character varying(40) NOT NULL,
    build_state_id bigint,
    source_build_type_id character varying(80),
    revision_rule character varying(80),
    branch character varying(255),
    src_paths character varying(40960)
);


ALTER TABLE public.build_artifact_dependency OWNER TO teamcity;

--
-- TOC entry 300 (class 1259 OID 17013)
-- Name: build_attrs; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_attrs (
    build_state_id bigint NOT NULL,
    attr_name character varying(70) NOT NULL,
    attr_value character varying(1000),
    attr_num_value bigint
);


ALTER TABLE public.build_attrs OWNER TO teamcity;

--
-- TOC entry 318 (class 1259 OID 17134)
-- Name: build_checkout_rules; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_checkout_rules (
    build_state_id bigint NOT NULL,
    vcs_root_id integer NOT NULL,
    checkout_rules character varying(16000)
);


ALTER TABLE public.build_checkout_rules OWNER TO teamcity;

--
-- TOC entry 301 (class 1259 OID 17022)
-- Name: build_data_storage; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_data_storage (
    build_id bigint NOT NULL,
    metric_id bigint NOT NULL,
    metric_value numeric(19,6) NOT NULL
);


ALTER TABLE public.build_data_storage OWNER TO teamcity;

--
-- TOC entry 299 (class 1259 OID 17007)
-- Name: build_dependency; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_dependency (
    build_state_id bigint NOT NULL,
    depends_on bigint NOT NULL,
    dependency_options integer
);


ALTER TABLE public.build_dependency OWNER TO teamcity;

--
-- TOC entry 340 (class 1259 OID 17275)
-- Name: build_labels; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_labels (
    build_id bigint NOT NULL,
    vcs_root_id integer NOT NULL,
    label character varying(80),
    status integer DEFAULT 0,
    error_message character varying(256)
);


ALTER TABLE public.build_labels OWNER TO teamcity;

--
-- TOC entry 345 (class 1259 OID 17311)
-- Name: build_overriden_roots; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_overriden_roots (
    build_state_id bigint NOT NULL,
    original_vcs_root_id integer NOT NULL,
    substitution_vcs_root_id integer NOT NULL
);


ALTER TABLE public.build_overriden_roots OWNER TO teamcity;

--
-- TOC entry 309 (class 1259 OID 17077)
-- Name: build_problem; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_problem (
    build_state_id bigint NOT NULL,
    problem_id integer NOT NULL,
    problem_description character varying(4000)
);


ALTER TABLE public.build_problem OWNER TO teamcity;

--
-- TOC entry 310 (class 1259 OID 17085)
-- Name: build_problem_attribute; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_problem_attribute (
    build_state_id bigint NOT NULL,
    problem_id integer NOT NULL,
    attr_name character varying(60) NOT NULL,
    attr_value character varying(2000) NOT NULL
);


ALTER TABLE public.build_problem_attribute OWNER TO teamcity;

--
-- TOC entry 327 (class 1259 OID 17192)
-- Name: build_problem_muted; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_problem_muted (
    build_state_id bigint NOT NULL,
    problem_id integer NOT NULL,
    mute_id integer
);


ALTER TABLE public.build_problem_muted OWNER TO teamcity;

--
-- TOC entry 298 (class 1259 OID 17001)
-- Name: build_project; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_project (
    build_id bigint NOT NULL,
    project_level integer NOT NULL,
    project_int_id character varying(80) NOT NULL
);


ALTER TABLE public.build_project OWNER TO teamcity;

--
-- TOC entry 330 (class 1259 OID 17210)
-- Name: build_queue; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_queue (
    build_type_id character varying(80),
    agent_restrictor_type_id integer,
    agent_restrictor_id integer,
    requestor character varying(1024),
    build_state_id bigint
);


ALTER TABLE public.build_queue OWNER TO teamcity;

--
-- TOC entry 331 (class 1259 OID 17216)
-- Name: build_queue_order; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_queue_order (
    version bigint NOT NULL,
    line_num integer NOT NULL,
    promotion_ids character varying(2000)
);


ALTER TABLE public.build_queue_order OWNER TO teamcity;

--
-- TOC entry 337 (class 1259 OID 17254)
-- Name: build_revisions; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_revisions (
    build_state_id bigint NOT NULL,
    vcs_root_id integer NOT NULL,
    vcs_revision character varying(200) NOT NULL,
    vcs_revision_display_name character varying(200),
    vcs_branch_name character varying(1024),
    modification_id bigint,
    vcs_root_type integer,
    checkout_mode integer
);


ALTER TABLE public.build_revisions OWNER TO teamcity;

--
-- TOC entry 363 (class 1259 OID 17423)
-- Name: build_set_tmp; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_set_tmp (
    build_id bigint NOT NULL
);


ALTER TABLE public.build_set_tmp OWNER TO teamcity;

--
-- TOC entry 294 (class 1259 OID 16949)
-- Name: build_state; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_state (
    id bigint NOT NULL,
    build_id bigint,
    build_type_id character varying(80),
    modification_id bigint,
    chain_modification_id bigint,
    personal_modification_id bigint,
    personal_user_id bigint,
    is_personal smallint DEFAULT 0 NOT NULL,
    is_canceled smallint DEFAULT 0 NOT NULL,
    is_changes_detached smallint DEFAULT 0 NOT NULL,
    is_deleted smallint DEFAULT 0 NOT NULL,
    branch_name character varying(1024),
    queued_time bigint,
    remove_from_queue_time bigint
);


ALTER TABLE public.build_state OWNER TO teamcity;

--
-- TOC entry 344 (class 1259 OID 17305)
-- Name: build_state_private_tag; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_state_private_tag (
    build_state_id bigint NOT NULL,
    owner bigint NOT NULL,
    tag character varying(255) NOT NULL
);


ALTER TABLE public.build_state_private_tag OWNER TO teamcity;

--
-- TOC entry 343 (class 1259 OID 17299)
-- Name: build_state_tag; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_state_tag (
    build_state_id bigint NOT NULL,
    tag character varying(255) NOT NULL
);


ALTER TABLE public.build_state_tag OWNER TO teamcity;

--
-- TOC entry 241 (class 1259 OID 16588)
-- Name: build_type; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_type (
    int_id character varying(80) NOT NULL,
    config_id character varying(80) NOT NULL,
    origin_project_id character varying(80),
    delete_time bigint
);


ALTER TABLE public.build_type OWNER TO teamcity;

--
-- TOC entry 246 (class 1259 OID 16623)
-- Name: build_type_counters; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_type_counters (
    build_type_id character varying(80) NOT NULL,
    counter bigint NOT NULL
);


ALTER TABLE public.build_type_counters OWNER TO teamcity;

--
-- TOC entry 314 (class 1259 OID 17112)
-- Name: build_type_edge_relation; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_type_edge_relation (
    child_modification_id bigint NOT NULL,
    build_type_id character varying(80) NOT NULL,
    parent_num integer NOT NULL,
    change_type integer
);


ALTER TABLE public.build_type_edge_relation OWNER TO teamcity;

--
-- TOC entry 317 (class 1259 OID 17129)
-- Name: build_type_group_vcs_change; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_type_group_vcs_change (
    modification_id bigint NOT NULL,
    group_id integer NOT NULL,
    change_type integer
);


ALTER TABLE public.build_type_group_vcs_change OWNER TO teamcity;

--
-- TOC entry 244 (class 1259 OID 16609)
-- Name: build_type_mapping; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_type_mapping (
    int_id character varying(80) NOT NULL,
    ext_id character varying(240) NOT NULL,
    main smallint NOT NULL
);


ALTER TABLE public.build_type_mapping OWNER TO teamcity;

--
-- TOC entry 313 (class 1259 OID 17106)
-- Name: build_type_vcs_change; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.build_type_vcs_change (
    modification_id bigint NOT NULL,
    build_type_id character varying(80) NOT NULL,
    change_type integer
);


ALTER TABLE public.build_type_vcs_change OWNER TO teamcity;

--
-- TOC entry 302 (class 1259 OID 17027)
-- Name: canceled_info; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.canceled_info (
    build_id bigint NOT NULL,
    user_id bigint,
    description character varying(256),
    interrupt_type integer
);


ALTER TABLE public.canceled_info OWNER TO teamcity;

--
-- TOC entry 364 (class 1259 OID 17428)
-- Name: clean_checkout_enforcement; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.clean_checkout_enforcement (
    build_type_id character varying(80) NOT NULL,
    agent_id integer NOT NULL,
    current_build_id bigint NOT NULL,
    request_time timestamp without time zone NOT NULL
);


ALTER TABLE public.clean_checkout_enforcement OWNER TO teamcity;

--
-- TOC entry 238 (class 1259 OID 16571)
-- Name: cleanup_history; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.cleanup_history (
    proc_id bigint NOT NULL,
    start_time bigint NOT NULL,
    finish_time bigint,
    interrupt_reason character varying(20)
);


ALTER TABLE public.cleanup_history OWNER TO teamcity;

--
-- TOC entry 254 (class 1259 OID 16675)
-- Name: cloud_image_state; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.cloud_image_state (
    project_id character varying(80) NOT NULL,
    profile_id character varying(30) NOT NULL,
    image_id character varying(80) NOT NULL,
    name character varying(80) NOT NULL
);


ALTER TABLE public.cloud_image_state OWNER TO teamcity;

--
-- TOC entry 258 (class 1259 OID 16697)
-- Name: cloud_image_without_agent; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.cloud_image_without_agent (
    profile_id character varying(30) NOT NULL,
    cloud_code character varying(6) NOT NULL,
    image_id character varying(80) NOT NULL,
    last_update timestamp without time zone NOT NULL
);


ALTER TABLE public.cloud_image_without_agent OWNER TO teamcity;

--
-- TOC entry 255 (class 1259 OID 16680)
-- Name: cloud_instance_state; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.cloud_instance_state (
    project_id character varying(80) NOT NULL,
    profile_id character varying(30) NOT NULL,
    image_id character varying(80) NOT NULL,
    instance_id character varying(80) NOT NULL,
    name character varying(80) NOT NULL,
    last_update timestamp without time zone NOT NULL,
    status character varying(30) NOT NULL,
    start_time timestamp without time zone NOT NULL,
    network_identity character varying(80),
    is_expired smallint,
    agent_id integer
);


ALTER TABLE public.cloud_instance_state OWNER TO teamcity;

--
-- TOC entry 257 (class 1259 OID 16692)
-- Name: cloud_started_instance; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.cloud_started_instance (
    profile_id character varying(30) NOT NULL,
    cloud_code character varying(6) NOT NULL,
    image_id character varying(80) NOT NULL,
    instance_id character varying(80) NOT NULL,
    last_update timestamp without time zone NOT NULL
);


ALTER TABLE public.cloud_started_instance OWNER TO teamcity;

--
-- TOC entry 256 (class 1259 OID 16685)
-- Name: cloud_state_data; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.cloud_state_data (
    project_id character varying(80) NOT NULL,
    profile_id character varying(30) NOT NULL,
    image_id character varying(80) NOT NULL,
    instance_id character varying(80) NOT NULL,
    data character varying(2000) NOT NULL
);


ALTER TABLE public.cloud_state_data OWNER TO teamcity;

--
-- TOC entry 360 (class 1259 OID 17403)
-- Name: comments; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.comments (
    id bigint NOT NULL,
    author_id bigint,
    when_changed bigint NOT NULL,
    commentary character varying(4096)
);


ALTER TABLE public.comments OWNER TO teamcity;

--
-- TOC entry 334 (class 1259 OID 17235)
-- Name: compiler_output; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.compiler_output (
    build_id bigint,
    message_order integer,
    message character varying(40960)
);


ALTER TABLE public.compiler_output OWNER TO teamcity;

--
-- TOC entry 374 (class 1259 OID 17493)
-- Name: config_persisting_tasks; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.config_persisting_tasks (
    id bigint NOT NULL,
    task_type character varying(20) NOT NULL,
    description character varying(2000),
    stage integer NOT NULL,
    node_id character varying(80) NOT NULL,
    created bigint NOT NULL,
    updated bigint DEFAULT 0 NOT NULL
);


ALTER TABLE public.config_persisting_tasks OWNER TO teamcity;

--
-- TOC entry 371 (class 1259 OID 17474)
-- Name: custom_data; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.custom_data (
    data_key_hash character varying(80) NOT NULL,
    collision_idx integer NOT NULL,
    data_domain character varying(80) NOT NULL,
    data_key character varying(2000) NOT NULL,
    data_id bigint NOT NULL
);


ALTER TABLE public.custom_data OWNER TO teamcity;

--
-- TOC entry 370 (class 1259 OID 17466)
-- Name: custom_data_body; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.custom_data_body (
    id bigint NOT NULL,
    part_num integer NOT NULL,
    total_parts integer NOT NULL,
    data_body character varying(2000),
    update_date bigint NOT NULL
);


ALTER TABLE public.custom_data_body OWNER TO teamcity;

--
-- TOC entry 278 (class 1259 OID 16837)
-- Name: data_storage_dict; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.data_storage_dict (
    metric_id bigint NOT NULL,
    value_type_key character varying(200)
);


ALTER TABLE public.data_storage_dict OWNER TO teamcity;

--
-- TOC entry 235 (class 1259 OID 16551)
-- Name: db_heartbeat; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.db_heartbeat (
    node_id character varying(80),
    starting_code bigint NOT NULL,
    starting_time timestamp without time zone,
    lock_mode character(1) NOT NULL,
    ip_address character varying(80),
    additional_info character varying(2000),
    last_time timestamp without time zone,
    update_interval bigint,
    uuid character varying(80),
    app_type character varying(80),
    url character varying(128),
    access_token character varying(80),
    build_number character varying(80),
    display_version character varying(80),
    responsibilities bigint,
    unix_last_time bigint,
    unix_starting_time bigint
);


ALTER TABLE public.db_heartbeat OWNER TO teamcity;

--
-- TOC entry 231 (class 1259 OID 16532)
-- Name: db_version; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.db_version (
    version_number integer NOT NULL,
    version_time timestamp without time zone NOT NULL,
    incompatible_change smallint DEFAULT 1 NOT NULL
);


ALTER TABLE public.db_version OWNER TO teamcity;

--
-- TOC entry 338 (class 1259 OID 17263)
-- Name: default_build_parameters; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.default_build_parameters (
    build_state_id bigint,
    param_name character varying(2000),
    param_value character varying(16000)
);


ALTER TABLE public.default_build_parameters OWNER TO teamcity;

--
-- TOC entry 239 (class 1259 OID 16576)
-- Name: domain_sequence; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.domain_sequence (
    domain_name character varying(100) NOT NULL,
    last_used_value bigint NOT NULL
);


ALTER TABLE public.domain_sequence OWNER TO teamcity;

--
-- TOC entry 336 (class 1259 OID 17247)
-- Name: downloaded_artifacts; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.downloaded_artifacts (
    target_build_id bigint,
    source_build_id bigint,
    download_timestamp bigint,
    artifact_path character varying(8192)
);


ALTER TABLE public.downloaded_artifacts OWNER TO teamcity;

--
-- TOC entry 356 (class 1259 OID 17382)
-- Name: duplicate_diff; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.duplicate_diff (
    build_id bigint NOT NULL,
    hash bigint NOT NULL
);


ALTER TABLE public.duplicate_diff OWNER TO teamcity;

--
-- TOC entry 357 (class 1259 OID 17387)
-- Name: duplicate_fragments; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.duplicate_fragments (
    id bigint NOT NULL,
    file_id bigint NOT NULL,
    line integer NOT NULL,
    offset_info character varying(100) NOT NULL
);


ALTER TABLE public.duplicate_fragments OWNER TO teamcity;

--
-- TOC entry 355 (class 1259 OID 17376)
-- Name: duplicate_results; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.duplicate_results (
    id bigint NOT NULL,
    build_id bigint NOT NULL,
    hash integer NOT NULL,
    cost integer
);


ALTER TABLE public.duplicate_results OWNER TO teamcity;

--
-- TOC entry 358 (class 1259 OID 17393)
-- Name: duplicate_stats; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.duplicate_stats (
    build_id bigint NOT NULL,
    total integer,
    new_total integer,
    old_total integer
);


ALTER TABLE public.duplicate_stats OWNER TO teamcity;

--
-- TOC entry 303 (class 1259 OID 17032)
-- Name: failed_tests; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.failed_tests (
    test_name_id bigint NOT NULL,
    build_id bigint NOT NULL,
    test_id integer NOT NULL,
    ffi_build_id bigint
);


ALTER TABLE public.failed_tests OWNER TO teamcity;

--
-- TOC entry 333 (class 1259 OID 17228)
-- Name: failed_tests_output; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.failed_tests_output (
    build_id bigint NOT NULL,
    test_id integer NOT NULL,
    problem_description character varying(256),
    std_output character varying(40960),
    error_output character varying(40960),
    stacktrace character varying(40960),
    expected character varying(40960),
    actual character varying(40960)
);


ALTER TABLE public.failed_tests_output OWNER TO teamcity;

--
-- TOC entry 312 (class 1259 OID 17099)
-- Name: final_artifact_dependency; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.final_artifact_dependency (
    artif_dep_id character varying(40) NOT NULL,
    build_state_id bigint,
    source_build_type_id character varying(80),
    revision_rule character varying(80),
    branch character varying(255),
    src_paths character varying(40960)
);


ALTER TABLE public.final_artifact_dependency OWNER TO teamcity;

--
-- TOC entry 373 (class 1259 OID 17489)
-- Name: hidden_health_item; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.hidden_health_item (
    item_id bigint NOT NULL,
    user_id bigint
);


ALTER TABLE public.hidden_health_item OWNER TO teamcity;

--
-- TOC entry 296 (class 1259 OID 16974)
-- Name: history; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.history (
    build_id bigint NOT NULL,
    agent_name character varying(256),
    build_type_id character varying(80),
    branch_name character varying(1024),
    build_start_time_server bigint,
    build_start_time_agent bigint,
    build_finish_time_server bigint,
    remove_from_queue_time bigint,
    queued_time bigint,
    status integer,
    status_text character varying(256),
    user_status_text character varying(256),
    pin integer,
    is_personal integer,
    is_canceled integer,
    build_number character varying(512),
    requestor character varying(1024),
    build_state_id bigint,
    agent_type_id integer
);


ALTER TABLE public.history OWNER TO teamcity;

--
-- TOC entry 315 (class 1259 OID 17118)
-- Name: ids_group; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.ids_group (
    id integer NOT NULL,
    group_hash character varying(80) NOT NULL
);


ALTER TABLE public.ids_group OWNER TO teamcity;

--
-- TOC entry 316 (class 1259 OID 17124)
-- Name: ids_group_entity_id; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.ids_group_entity_id (
    group_id integer NOT NULL,
    entity_id character varying(160) NOT NULL
);


ALTER TABLE public.ids_group_entity_id OWNER TO teamcity;

--
-- TOC entry 335 (class 1259 OID 17241)
-- Name: ignored_tests; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.ignored_tests (
    build_id bigint,
    test_id integer,
    ignore_reason character varying(2000)
);


ALTER TABLE public.ignored_tests OWNER TO teamcity;

--
-- TOC entry 349 (class 1259 OID 17339)
-- Name: inspection_data; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.inspection_data (
    hash bigint NOT NULL,
    result character varying(4000),
    severity integer,
    type_pattern integer,
    fqname character varying(4000),
    file_name character varying(255),
    parent_fqnames character varying(4000),
    parent_type_patterns character varying(20),
    module_name character varying(40),
    inspection_id bigint,
    is_local integer,
    used integer DEFAULT 1 NOT NULL
);


ALTER TABLE public.inspection_data OWNER TO teamcity;

--
-- TOC entry 353 (class 1259 OID 17363)
-- Name: inspection_diff; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.inspection_diff (
    build_id bigint NOT NULL,
    hash bigint NOT NULL
);


ALTER TABLE public.inspection_diff OWNER TO teamcity;

--
-- TOC entry 350 (class 1259 OID 17349)
-- Name: inspection_fixes; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.inspection_fixes (
    hash bigint NOT NULL,
    hint character varying(255)
);


ALTER TABLE public.inspection_fixes OWNER TO teamcity;

--
-- TOC entry 348 (class 1259 OID 17330)
-- Name: inspection_info; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.inspection_info (
    id bigint NOT NULL,
    inspection_id character varying(255),
    inspection_name character varying(255),
    inspection_desc character varying(4000),
    group_name character varying(255)
);


ALTER TABLE public.inspection_info OWNER TO teamcity;

--
-- TOC entry 351 (class 1259 OID 17353)
-- Name: inspection_results; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.inspection_results (
    build_id bigint NOT NULL,
    hash bigint NOT NULL,
    line integer NOT NULL
);


ALTER TABLE public.inspection_results OWNER TO teamcity;

--
-- TOC entry 352 (class 1259 OID 17358)
-- Name: inspection_stats; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.inspection_stats (
    build_id bigint NOT NULL,
    total integer,
    new_total integer,
    old_total integer,
    errors integer,
    new_errors integer,
    old_errors integer
);


ALTER TABLE public.inspection_stats OWNER TO teamcity;

--
-- TOC entry 377 (class 1259 OID 17514)
-- Name: light_history; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.light_history (
    build_id bigint NOT NULL,
    agent_name character varying(80),
    build_type_id character varying(80),
    build_start_time_server bigint,
    build_start_time_agent bigint,
    build_finish_time_server bigint,
    status integer,
    status_text character varying(256),
    user_status_text character varying(256),
    pin integer,
    is_personal integer,
    is_canceled integer,
    build_number character varying(256),
    requestor character varying(1024),
    queued_time bigint,
    remove_from_queue_time bigint,
    build_state_id bigint,
    agent_type_id integer,
    branch_name character varying(255)
);


ALTER TABLE public.light_history OWNER TO teamcity;

--
-- TOC entry 276 (class 1259 OID 16822)
-- Name: long_file_name; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.long_file_name (
    hash character varying(40) NOT NULL,
    file_name character varying(16000) NOT NULL
);


ALTER TABLE public.long_file_name OWNER TO teamcity;

--
-- TOC entry 232 (class 1259 OID 16538)
-- Name: meta_file_line; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.meta_file_line (
    file_name character varying(15) NOT NULL,
    line_nr integer NOT NULL,
    line_text character varying(160)
);


ALTER TABLE public.meta_file_line OWNER TO teamcity;

--
-- TOC entry 319 (class 1259 OID 17141)
-- Name: mute_info; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.mute_info (
    mute_id integer NOT NULL,
    muting_user_id bigint NOT NULL,
    muting_time timestamp without time zone NOT NULL,
    muting_comment character varying(2000),
    scope character(1) NOT NULL,
    project_int_id character varying(80) NOT NULL,
    build_id bigint,
    unmute_when_fixed smallint,
    unmute_by_time timestamp without time zone
);


ALTER TABLE public.mute_info OWNER TO teamcity;

--
-- TOC entry 320 (class 1259 OID 17150)
-- Name: mute_info_bt; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.mute_info_bt (
    mute_id integer NOT NULL,
    build_type_id character varying(80) NOT NULL
);


ALTER TABLE public.mute_info_bt OWNER TO teamcity;

--
-- TOC entry 324 (class 1259 OID 17175)
-- Name: mute_info_problem; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.mute_info_problem (
    mute_id integer NOT NULL,
    problem_id integer NOT NULL
);


ALTER TABLE public.mute_info_problem OWNER TO teamcity;

--
-- TOC entry 321 (class 1259 OID 17156)
-- Name: mute_info_test; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.mute_info_test (
    mute_id integer NOT NULL,
    test_name_id bigint NOT NULL
);


ALTER TABLE public.mute_info_test OWNER TO teamcity;

--
-- TOC entry 326 (class 1259 OID 17186)
-- Name: mute_problem_in_bt; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.mute_problem_in_bt (
    mute_id integer NOT NULL,
    build_type_id character varying(80) NOT NULL,
    problem_id integer NOT NULL
);


ALTER TABLE public.mute_problem_in_bt OWNER TO teamcity;

--
-- TOC entry 325 (class 1259 OID 17180)
-- Name: mute_problem_in_proj; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.mute_problem_in_proj (
    mute_id integer NOT NULL,
    project_int_id character varying(80) NOT NULL,
    problem_id integer NOT NULL
);


ALTER TABLE public.mute_problem_in_proj OWNER TO teamcity;

--
-- TOC entry 323 (class 1259 OID 17168)
-- Name: mute_test_in_bt; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.mute_test_in_bt (
    mute_id integer NOT NULL,
    build_type_id character varying(80) NOT NULL,
    test_name_id bigint NOT NULL
);


ALTER TABLE public.mute_test_in_bt OWNER TO teamcity;

--
-- TOC entry 322 (class 1259 OID 17161)
-- Name: mute_test_in_proj; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.mute_test_in_proj (
    mute_id integer NOT NULL,
    project_int_id character varying(80) NOT NULL,
    test_name_id bigint NOT NULL
);


ALTER TABLE public.mute_test_in_proj OWNER TO teamcity;

--
-- TOC entry 366 (class 1259 OID 17437)
-- Name: node_events; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.node_events (
    id bigint NOT NULL,
    name character varying(64),
    long_arg1 bigint,
    long_arg2 bigint,
    str_arg character varying(255),
    node_id character varying(80) NOT NULL,
    created timestamp without time zone
);


ALTER TABLE public.node_events OWNER TO teamcity;

--
-- TOC entry 369 (class 1259 OID 17460)
-- Name: node_locks; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.node_locks (
    lock_type character varying(64) NOT NULL,
    lock_arg character varying(80),
    id bigint NOT NULL,
    node_id character varying(80) NOT NULL,
    state integer NOT NULL,
    created bigint NOT NULL
);


ALTER TABLE public.node_locks OWNER TO teamcity;

--
-- TOC entry 367 (class 1259 OID 17443)
-- Name: node_tasks; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.node_tasks (
    id integer NOT NULL,
    task_type character varying(64) NOT NULL,
    task_identity character varying(255) NOT NULL,
    long_arg1 bigint,
    long_arg2 bigint,
    str_arg character varying(1024),
    long_str_arg_uuid character varying(40),
    node_id character varying(80) NOT NULL,
    executor_node_id character varying(80),
    task_state integer NOT NULL,
    result character varying(1024),
    long_result_uuid character varying(40),
    created timestamp without time zone,
    finished timestamp without time zone,
    last_activity timestamp without time zone
);


ALTER TABLE public.node_tasks OWNER TO teamcity;

--
-- TOC entry 368 (class 1259 OID 17453)
-- Name: node_tasks_long_value; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.node_tasks_long_value (
    uuid character varying(40) NOT NULL,
    long_value text NOT NULL
);


ALTER TABLE public.node_tasks_long_value OWNER TO teamcity;

--
-- TOC entry 275 (class 1259 OID 16816)
-- Name: permanent_token_permissions; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.permanent_token_permissions (
    id bigint NOT NULL,
    project_id character varying(80) NOT NULL,
    permission integer NOT NULL
);


ALTER TABLE public.permanent_token_permissions OWNER TO teamcity;

--
-- TOC entry 274 (class 1259 OID 16804)
-- Name: permanent_tokens; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.permanent_tokens (
    id bigint NOT NULL,
    identifier character varying(36) NOT NULL,
    name character varying(128) NOT NULL,
    user_id bigint NOT NULL,
    hashed_value character varying(255) NOT NULL,
    expiration_time bigint,
    creation_time bigint,
    last_access_time bigint,
    last_access_info character varying(255)
);


ALTER TABLE public.permanent_tokens OWNER TO teamcity;

--
-- TOC entry 341 (class 1259 OID 17282)
-- Name: personal_build_relative_path; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.personal_build_relative_path (
    build_id bigint,
    original_path_hash bigint,
    relative_path character varying(16000)
);


ALTER TABLE public.personal_build_relative_path OWNER TO teamcity;

--
-- TOC entry 289 (class 1259 OID 16916)
-- Name: personal_vcs_change; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.personal_vcs_change (
    modification_id bigint NOT NULL,
    file_num integer NOT NULL,
    vcs_file_name character varying(2000) NOT NULL,
    vcs_file_name_hash character varying(40),
    relative_file_name_pos integer,
    relative_file_name character varying(2000),
    relative_file_name_hash character varying(40),
    change_type integer NOT NULL,
    change_name character varying(64),
    before_revision character varying(200),
    after_revision character varying(200)
);


ALTER TABLE public.personal_vcs_change OWNER TO teamcity;

--
-- TOC entry 376 (class 1259 OID 17508)
-- Name: personal_vcs_changes; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.personal_vcs_changes (
    modification_id bigint,
    change_name character varying(64),
    change_type integer,
    before_revision character varying(2048),
    after_revision character varying(2048),
    vcs_file_name character varying(16000),
    relative_file_name character varying(16000)
);


ALTER TABLE public.personal_vcs_changes OWNER TO teamcity;

--
-- TOC entry 288 (class 1259 OID 16904)
-- Name: personal_vcs_history; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.personal_vcs_history (
    modification_id bigint NOT NULL,
    modification_hash character varying(40) NOT NULL,
    user_id bigint NOT NULL,
    description character varying(2000),
    change_date bigint NOT NULL,
    changes_count integer NOT NULL,
    commit_changes integer,
    status integer DEFAULT 0 NOT NULL,
    scheduled_for_deletion smallint DEFAULT 0 NOT NULL
);


ALTER TABLE public.personal_vcs_history OWNER TO teamcity;

--
-- TOC entry 279 (class 1259 OID 16844)
-- Name: problem; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.problem (
    problem_id integer NOT NULL,
    problem_type character varying(80) NOT NULL,
    problem_identity character varying(60) NOT NULL
);


ALTER TABLE public.problem OWNER TO teamcity;

--
-- TOC entry 240 (class 1259 OID 16581)
-- Name: project; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.project (
    int_id character varying(80) NOT NULL,
    config_id character varying(80) NOT NULL,
    origin_project_id character varying(80),
    delete_time bigint
);


ALTER TABLE public.project OWNER TO teamcity;

--
-- TOC entry 354 (class 1259 OID 17369)
-- Name: project_files; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.project_files (
    file_id bigint NOT NULL,
    file_name character varying(255) NOT NULL
);


ALTER TABLE public.project_files OWNER TO teamcity;

--
-- TOC entry 243 (class 1259 OID 16602)
-- Name: project_mapping; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.project_mapping (
    int_id character varying(80) NOT NULL,
    ext_id character varying(240) NOT NULL,
    main smallint NOT NULL
);


ALTER TABLE public.project_mapping OWNER TO teamcity;

--
-- TOC entry 273 (class 1259 OID 16799)
-- Name: remember_me; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.remember_me (
    user_key character varying(65) NOT NULL,
    secure bigint NOT NULL
);


ALTER TABLE public.remember_me OWNER TO teamcity;

--
-- TOC entry 297 (class 1259 OID 16990)
-- Name: removed_builds_history; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.removed_builds_history (
    build_id bigint NOT NULL,
    agent_name character varying(256),
    build_type_id character varying(80),
    build_start_time_server bigint,
    build_start_time_agent bigint,
    build_finish_time_server bigint,
    status integer,
    status_text character varying(256),
    user_status_text character varying(256),
    pin integer,
    is_personal integer,
    is_canceled integer,
    build_number character varying(512),
    requestor character varying(1024),
    queued_time bigint,
    remove_from_queue_time bigint,
    build_state_id bigint,
    agent_type_id integer,
    branch_name character varying(1024)
);


ALTER TABLE public.removed_builds_history OWNER TO teamcity;

--
-- TOC entry 342 (class 1259 OID 17289)
-- Name: responsibilities; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.responsibilities (
    problem_id character varying(80) NOT NULL,
    state integer NOT NULL,
    responsible_user_id bigint NOT NULL,
    reporter_user_id bigint,
    timestmp bigint,
    remove_method integer DEFAULT 0 NOT NULL,
    comments character varying(4096)
);


ALTER TABLE public.responsibilities OWNER TO teamcity;

--
-- TOC entry 295 (class 1259 OID 16966)
-- Name: running; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.running (
    build_id bigint NOT NULL,
    agent_id integer,
    build_type_id character varying(80),
    build_start_time_agent bigint,
    build_start_time_server bigint,
    build_finish_time_server bigint,
    last_build_activity_time bigint,
    is_personal integer,
    build_number character varying(512),
    build_counter bigint,
    requestor character varying(1024),
    access_code character varying(60),
    queued_ag_restr_type_id integer,
    queued_ag_restr_id integer,
    build_state_id bigint,
    agent_type_id integer,
    user_status_text character varying(256),
    progress_text character varying(1024),
    current_path_text character varying(2048)
);


ALTER TABLE public.running OWNER TO teamcity;

--
-- TOC entry 378 (class 1259 OID 17525)
-- Name: server; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.server (
    server_id bigint
);


ALTER TABLE public.server OWNER TO teamcity;

--
-- TOC entry 372 (class 1259 OID 17483)
-- Name: server_health_items; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.server_health_items (
    id bigint NOT NULL,
    report_id character varying(80) NOT NULL,
    category_id character varying(80) NOT NULL,
    item_id character varying(255) NOT NULL
);


ALTER TABLE public.server_health_items OWNER TO teamcity;

--
-- TOC entry 234 (class 1259 OID 16546)
-- Name: server_property; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.server_property (
    prop_name character varying(80) NOT NULL,
    prop_value character varying(256) NOT NULL
);


ALTER TABLE public.server_property OWNER TO teamcity;

--
-- TOC entry 365 (class 1259 OID 17433)
-- Name: server_statistics; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.server_statistics (
    metric_key bigint NOT NULL,
    metric_value bigint NOT NULL,
    metric_timestamp bigint NOT NULL
);


ALTER TABLE public.server_statistics OWNER TO teamcity;

--
-- TOC entry 233 (class 1259 OID 16543)
-- Name: single_row; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.single_row (
    dummy_field character(1)
);


ALTER TABLE public.single_row OWNER TO teamcity;

--
-- TOC entry 332 (class 1259 OID 17223)
-- Name: stats; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.stats (
    build_id bigint NOT NULL,
    test_count integer
);


ALTER TABLE public.stats OWNER TO teamcity;

--
-- TOC entry 359 (class 1259 OID 17398)
-- Name: stats_publisher_state; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.stats_publisher_state (
    metric_id bigint NOT NULL,
    value bigint NOT NULL
);


ALTER TABLE public.stats_publisher_state OWNER TO teamcity;

--
-- TOC entry 329 (class 1259 OID 17204)
-- Name: test_failure_rate; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.test_failure_rate (
    build_type_id character varying(80) NOT NULL,
    test_name_id bigint NOT NULL,
    success_count integer,
    failure_count integer,
    last_failure_time bigint
);


ALTER TABLE public.test_failure_rate OWNER TO teamcity;

--
-- TOC entry 304 (class 1259 OID 17039)
-- Name: test_info; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.test_info (
    build_id bigint NOT NULL,
    test_id integer NOT NULL,
    test_name_id bigint,
    status integer,
    duration integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.test_info OWNER TO teamcity;

--
-- TOC entry 305 (class 1259 OID 17046)
-- Name: test_info_archive; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.test_info_archive (
    build_id bigint NOT NULL,
    test_id integer NOT NULL,
    test_name_id bigint NOT NULL,
    status integer,
    duration integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.test_info_archive OWNER TO teamcity;

--
-- TOC entry 308 (class 1259 OID 17069)
-- Name: test_metadata; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.test_metadata (
    build_id bigint NOT NULL,
    test_id integer NOT NULL,
    test_name_id bigint NOT NULL,
    key_id bigint NOT NULL,
    type_id integer,
    str_value character varying(1024),
    num_value numeric(19,6)
);


ALTER TABLE public.test_metadata OWNER TO teamcity;

--
-- TOC entry 306 (class 1259 OID 17053)
-- Name: test_metadata_dict; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.test_metadata_dict (
    key_id bigint NOT NULL,
    name_digest character varying(32) NOT NULL,
    name character varying(512) NOT NULL
);


ALTER TABLE public.test_metadata_dict OWNER TO teamcity;

--
-- TOC entry 307 (class 1259 OID 17062)
-- Name: test_metadata_types; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.test_metadata_types (
    type_id integer NOT NULL,
    name character varying(64) NOT NULL
);


ALTER TABLE public.test_metadata_types OWNER TO teamcity;

--
-- TOC entry 328 (class 1259 OID 17198)
-- Name: test_muted; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.test_muted (
    build_id bigint NOT NULL,
    test_name_id bigint NOT NULL,
    mute_id integer NOT NULL
);


ALTER TABLE public.test_muted OWNER TO teamcity;

--
-- TOC entry 277 (class 1259 OID 16829)
-- Name: test_names; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.test_names (
    id bigint NOT NULL,
    test_name character varying(1024) NOT NULL,
    order_num bigint
);


ALTER TABLE public.test_names OWNER TO teamcity;

--
-- TOC entry 263 (class 1259 OID 16735)
-- Name: user_attribute; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_attribute (
    user_id bigint NOT NULL,
    attr_key character varying(80) NOT NULL,
    attr_value character varying(2000)
);


ALTER TABLE public.user_attribute OWNER TO teamcity;

--
-- TOC entry 264 (class 1259 OID 16742)
-- Name: user_blocks; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_blocks (
    user_id bigint NOT NULL,
    block_type character varying(80) NOT NULL,
    state character varying(2048)
);


ALTER TABLE public.user_blocks OWNER TO teamcity;

--
-- TOC entry 339 (class 1259 OID 17269)
-- Name: user_build_parameters; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_build_parameters (
    build_state_id bigint,
    param_name character varying(2000),
    param_value character varying(16000)
);


ALTER TABLE public.user_build_parameters OWNER TO teamcity;

--
-- TOC entry 283 (class 1259 OID 16869)
-- Name: user_build_types_order; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_build_types_order (
    user_id bigint NOT NULL,
    project_int_id character varying(80) NOT NULL,
    bt_int_id character varying(80) NOT NULL,
    ordernum integer NOT NULL,
    visible integer NOT NULL
);


ALTER TABLE public.user_build_types_order OWNER TO teamcity;

--
-- TOC entry 267 (class 1259 OID 16761)
-- Name: user_notification_data; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_notification_data (
    user_id bigint NOT NULL,
    rule_id bigint NOT NULL,
    additional_data character varying(2000)
);


ALTER TABLE public.user_notification_data OWNER TO teamcity;

--
-- TOC entry 265 (class 1259 OID 16749)
-- Name: user_notification_events; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_notification_events (
    id bigint NOT NULL,
    user_id bigint NOT NULL,
    notificator_type character varying(20) NOT NULL,
    events_mask integer NOT NULL
);


ALTER TABLE public.user_notification_events OWNER TO teamcity;

--
-- TOC entry 282 (class 1259 OID 16863)
-- Name: user_projects_order; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_projects_order (
    user_id bigint NOT NULL,
    project_int_id character varying(80) NOT NULL,
    ordernum integer
);


ALTER TABLE public.user_projects_order OWNER TO teamcity;

--
-- TOC entry 281 (class 1259 OID 16857)
-- Name: user_projects_visibility; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_projects_visibility (
    user_id bigint NOT NULL,
    project_int_id character varying(80) NOT NULL,
    visible integer NOT NULL
);


ALTER TABLE public.user_projects_visibility OWNER TO teamcity;

--
-- TOC entry 262 (class 1259 OID 16727)
-- Name: user_property; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_property (
    user_id bigint NOT NULL,
    prop_key character varying(80) NOT NULL,
    prop_value character varying(2000),
    locase_value_hash bigint
);


ALTER TABLE public.user_property OWNER TO teamcity;

--
-- TOC entry 346 (class 1259 OID 17318)
-- Name: user_roles; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_roles (
    user_id bigint NOT NULL,
    role_id character varying(80) NOT NULL,
    project_int_id character varying(80)
);


ALTER TABLE public.user_roles OWNER TO teamcity;

--
-- TOC entry 266 (class 1259 OID 16756)
-- Name: user_watch_type; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.user_watch_type (
    rule_id bigint NOT NULL,
    user_id bigint NOT NULL,
    notificator_type character varying(20) NOT NULL,
    watch_type integer NOT NULL,
    watch_value character varying(80) NOT NULL,
    order_num bigint
);


ALTER TABLE public.user_watch_type OWNER TO teamcity;

--
-- TOC entry 272 (class 1259 OID 16791)
-- Name: usergroup_notification_data; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.usergroup_notification_data (
    group_id character varying(80) NOT NULL,
    rule_id bigint NOT NULL,
    additional_data character varying(2000)
);


ALTER TABLE public.usergroup_notification_data OWNER TO teamcity;

--
-- TOC entry 270 (class 1259 OID 16779)
-- Name: usergroup_notification_events; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.usergroup_notification_events (
    id bigint NOT NULL,
    group_id character varying(80) NOT NULL,
    notificator_type character varying(20) NOT NULL,
    events_mask integer NOT NULL
);


ALTER TABLE public.usergroup_notification_events OWNER TO teamcity;

--
-- TOC entry 260 (class 1259 OID 16711)
-- Name: usergroup_property; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.usergroup_property (
    group_id character varying(80) NOT NULL,
    prop_key character varying(80) NOT NULL,
    prop_value character varying(2000)
);


ALTER TABLE public.usergroup_property OWNER TO teamcity;

--
-- TOC entry 347 (class 1259 OID 17324)
-- Name: usergroup_roles; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.usergroup_roles (
    group_id character varying(80) NOT NULL,
    role_id character varying(80) NOT NULL,
    project_int_id character varying(80)
);


ALTER TABLE public.usergroup_roles OWNER TO teamcity;

--
-- TOC entry 268 (class 1259 OID 16769)
-- Name: usergroup_subgroups; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.usergroup_subgroups (
    hostgroup_id character varying(80) NOT NULL,
    subgroup_id character varying(80) NOT NULL
);


ALTER TABLE public.usergroup_subgroups OWNER TO teamcity;

--
-- TOC entry 269 (class 1259 OID 16774)
-- Name: usergroup_users; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.usergroup_users (
    group_id character varying(80) NOT NULL,
    user_id bigint NOT NULL
);


ALTER TABLE public.usergroup_users OWNER TO teamcity;

--
-- TOC entry 271 (class 1259 OID 16786)
-- Name: usergroup_watch_type; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.usergroup_watch_type (
    rule_id bigint NOT NULL,
    group_id character varying(80) NOT NULL,
    notificator_type character varying(20) NOT NULL,
    watch_type integer NOT NULL,
    watch_value character varying(80) NOT NULL,
    order_num bigint
);


ALTER TABLE public.usergroup_watch_type OWNER TO teamcity;

--
-- TOC entry 259 (class 1259 OID 16702)
-- Name: usergroups; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.usergroups (
    group_id character varying(80) NOT NULL,
    name character varying(255) NOT NULL,
    description character varying(2000)
);


ALTER TABLE public.usergroups OWNER TO teamcity;

--
-- TOC entry 261 (class 1259 OID 16718)
-- Name: users; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.users (
    id bigint NOT NULL,
    username character varying(60) NOT NULL,
    password character varying(128),
    name character varying(256),
    email character varying(256),
    last_login_timestamp bigint,
    algorithm character varying(20)
);


ALTER TABLE public.users OWNER TO teamcity;

--
-- TOC entry 287 (class 1259 OID 16897)
-- Name: vcs_change; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_change (
    modification_id bigint NOT NULL,
    file_num integer NOT NULL,
    vcs_file_name character varying(2000) NOT NULL,
    vcs_file_name_hash character varying(40),
    relative_file_name_pos integer,
    relative_file_name character varying(2000),
    relative_file_name_hash character varying(40),
    change_type integer NOT NULL,
    change_name character varying(64),
    before_revision character varying(200),
    after_revision character varying(200)
);


ALTER TABLE public.vcs_change OWNER TO teamcity;

--
-- TOC entry 291 (class 1259 OID 16929)
-- Name: vcs_change_attrs; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_change_attrs (
    modification_id bigint NOT NULL,
    attr_name character varying(200) NOT NULL,
    attr_value character varying(1000)
);


ALTER TABLE public.vcs_change_attrs OWNER TO teamcity;

--
-- TOC entry 375 (class 1259 OID 17502)
-- Name: vcs_changes; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_changes (
    modification_id bigint,
    change_name character varying(64),
    change_type integer,
    before_revision character varying(2048),
    after_revision character varying(2048),
    vcs_file_name character varying(16000),
    relative_file_name character varying(16000)
);


ALTER TABLE public.vcs_changes OWNER TO teamcity;

--
-- TOC entry 290 (class 1259 OID 16923)
-- Name: vcs_changes_graph; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_changes_graph (
    child_modification_id bigint NOT NULL,
    child_revision character varying(200) NOT NULL,
    parent_num integer NOT NULL,
    parent_modification_id bigint,
    parent_revision character varying(200) NOT NULL
);


ALTER TABLE public.vcs_changes_graph OWNER TO teamcity;

--
-- TOC entry 286 (class 1259 OID 16888)
-- Name: vcs_history; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_history (
    modification_id bigint NOT NULL,
    user_name character varying(255),
    description character varying(2000),
    change_date bigint,
    register_date bigint,
    vcs_root_id integer,
    changes_count integer,
    version character varying(200) NOT NULL,
    display_version character varying(200)
);


ALTER TABLE public.vcs_history OWNER TO teamcity;

--
-- TOC entry 242 (class 1259 OID 16595)
-- Name: vcs_root; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_root (
    int_id integer NOT NULL,
    config_id character varying(80) NOT NULL,
    origin_project_id character varying(80),
    delete_time bigint
);


ALTER TABLE public.vcs_root OWNER TO teamcity;

--
-- TOC entry 292 (class 1259 OID 16936)
-- Name: vcs_root_first_revision; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_root_first_revision (
    build_type_id character varying(80) NOT NULL,
    parent_root_id integer NOT NULL,
    settings_hash bigint NOT NULL,
    vcs_revision character varying(200) NOT NULL
);


ALTER TABLE public.vcs_root_first_revision OWNER TO teamcity;

--
-- TOC entry 284 (class 1259 OID 16875)
-- Name: vcs_root_instance; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_root_instance (
    id integer NOT NULL,
    parent_id integer NOT NULL,
    settings_hash bigint NOT NULL,
    body character varying(16384)
);


ALTER TABLE public.vcs_root_instance OWNER TO teamcity;

--
-- TOC entry 245 (class 1259 OID 16616)
-- Name: vcs_root_mapping; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_root_mapping (
    int_id integer NOT NULL,
    ext_id character varying(240) NOT NULL,
    main smallint NOT NULL
);


ALTER TABLE public.vcs_root_mapping OWNER TO teamcity;

--
-- TOC entry 293 (class 1259 OID 16941)
-- Name: vcs_username; Type: TABLE; Schema: public; Owner: teamcity
--

CREATE TABLE public.vcs_username (
    user_id bigint NOT NULL,
    vcs_name character varying(60) NOT NULL,
    parent_vcs_root_id integer NOT NULL,
    order_num integer NOT NULL,
    username character varying(255) NOT NULL
);


ALTER TABLE public.vcs_username OWNER TO teamcity;

--
-- TOC entry 6399 (class 0 OID 17411)
-- Dependencies: 361
-- Data for Name: action_history; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.action_history (object_id, comment_id, action, additional_data) FROM stdin;
1	1	15	\N
project1	2	19	\N
1	3	24	\N
bt1	4	17	\N
bt1	5	88	\N
bt1	6	88	\N
NuGet.CommandLine.6.8.0	7	153	\N
NuGet.CommandLine.6.8.0	8	155	\N
project1	9	87	\N
1	10	38	bt1
1	11	11	\N
1	12	146	\N
bt1	13	88	\N
1	14	145	\N
server	101	170	\N
server	102	171	\N
bt1	103	88	\N
bt1	104	88	\N
bt1	201	88	\N
bt1	202	88	\N
\.


--
-- TOC entry 6291 (class 0 OID 16663)
-- Dependencies: 253
-- Data for Name: agent; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.agent (id, name, host_addr, port, agent_type_id, status, authorized, registered, registration_timestamp, last_binding_timestamp, unregistered_reason, authorization_token, status_to_restore, status_restoring_timestamp, version, plugins_version) FROM stdin;
1	Default Agent	172.17.0.1	9090	1	1	1	1	1710285417718	1710289930682	\N	7df777635b4c712635c0ff1c9a758b34	\N	\N	147512	147512-md5-2f7381755eb17d1d4895bffaf7b5f9db
\.


--
-- TOC entry 6285 (class 0 OID 16628)
-- Dependencies: 247
-- Data for Name: agent_pool; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.agent_pool (agent_pool_id, agent_pool_name, min_agents, max_agents, owner_project_id) FROM stdin;
0	Default	\N	\N	\N
\.


--
-- TOC entry 6323 (class 0 OID 16883)
-- Dependencies: 285
-- Data for Name: agent_pool_project; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.agent_pool_project (agent_pool_id, project_int_id) FROM stdin;
0	_Root
0	project1
\.


--
-- TOC entry 6286 (class 0 OID 16633)
-- Dependencies: 248
-- Data for Name: agent_type; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.agent_type (agent_type_id, agent_pool_id, cloud_code, profile_id, image_id, policy) FROM stdin;
1	0	A	A	real-1	1
\.


--
-- TOC entry 6318 (class 0 OID 16851)
-- Dependencies: 280
-- Data for Name: agent_type_bt_access; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.agent_type_bt_access (agent_type_id, build_type_id) FROM stdin;
\.


--
-- TOC entry 6287 (class 0 OID 16641)
-- Dependencies: 249
-- Data for Name: agent_type_info; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.agent_type_info (agent_type_id, os_name, cpu_rank, created_timestamp, modified_timestamp) FROM stdin;
1	Windows 11, version 10.0	934	2024-03-13 00:27:42.247	2024-03-13 00:27:42.247
\.


--
-- TOC entry 6290 (class 0 OID 16656)
-- Dependencies: 252
-- Data for Name: agent_type_param; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.agent_type_param (agent_type_id, param_kind, param_name, param_value) FROM stdin;
1	B	env.NUMBER_OF_PROCESSORS	16
1	B	env.USERPROFILE	C:\\WINDOWS\\system32\\config\\systemprofile
1	B	env.PROCESSOR_ARCHITECTURE	AMD64
1	B	system.teamcity.dotnet.nunitlauncher.msbuild.task	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\dotnetPlugin\\bin\\JetBrains.BuildServer.MSBuildLoggers.dll
1	B	system.agent.work.dir	C:\\code\\cicd\\teamcity\\BuildAgent\\work
1	B	system.FxCopRoot	C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Team Tools\\Static Analysis Tools\\FxCop
1	B	env.GTK_BASEPATH	C:\\Program Files (x86)\\GtkSharp\\2.12\\
1	B	env.CommonProgramW6432	C:\\Program Files\\Common Files
1	B	env.JDK_1_8	C:\\Program Files (x86)\\Java\\jdk1.8.0_131
1	B	env.OS	Windows_NT
1	B	env.SystemRoot	C:\\WINDOWS
1	B	system.teamcity.dotnet.platform	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\dotnetPlugin\\bin\\JetBrains.TeamCity.PlatformProcessRunner.1.1.exe
1	B	env.JDK_18_x64	C:\\Program Files\\Java\\jdk1.8.0_131
1	B	system.teamcity.dotnet.nunitlauncher2.0	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\dotnetPlugin\\bin\\JetBrains.BuildServer.NUnitLauncher2.0.exe
1	B	env.USERDOMAIN	WORKGROUP
1	B	env.ComSpec	C:\\WINDOWS\\system32\\cmd.exe
1	B	env.WRAPPER_PATH_SEPARATOR	;
1	B	env.MSMPI_BIN	C:\\Program Files\\Microsoft MPI\\Bin\\
1	B	env.JAVA_HOME	C:\\Program Files\\Java\\jdk1.8.0_131
1	B	env.JRE_HOME	C:\\Program Files (x86)\\Java\\jdk1.8.0_131
1	B	env.ProgramFiles	C:\\Program Files
1	B	env.TEAMCITY_GIT_PATH	C:\\Program Files\\Git\\bin\\git.exe
1	B	env.ALLUSERSPROFILE	C:\\ProgramData
1	B	system.teamcity.dotnet.nunitlauncher2.0.vsts	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\dotnetPlugin\\bin\\JetBrains.BuildServer.NUnitLauncher2.0.VSTS.exe
1	B	system.teamcity.dotnet.nunitaddin	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\dotnetPlugin\\bin\\JetBrains.TeamCity.NUnitAddin-NUnit
1	B	env.LOCALAPPDATA	C:\\WINDOWS\\system32\\config\\systemprofile\\AppData\\Local
1	B	env.PROCESSOR_REVISION	0802
1	B	system.FxCopCmdFileVersion	17.0.34031.110
1	B	env.TEAMCITY_GIT_VERSION	2.42.0.0
1	B	env.PSModulePath	C:\\Program Files\\WindowsPowerShell\\Modules;C:\\WINDOWS\\system32\\WindowsPowerShell\\v1.0\\Modules
1	B	env.FSHARPINSTALLDIR	C:\\Program Files (x86)\\Microsoft SDKs\\F#\\4.1\\Framework\\v4.0\\
1	B	env.windir	C:\\WINDOWS
1	B	env.JDK_18	C:\\Program Files (x86)\\Java\\jdk1.8.0_131
1	B	env.WRAPPER_BITS	32
1	B	env.PROCESSOR_IDENTIFIER	AMD64 Family 23 Model 8 Stepping 2, AuthenticAMD
1	B	system.teamcity.dotnet.nunitlauncher	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\dotnetPlugin\\bin\\JetBrains.BuildServer.NUnitLauncher.exe
1	B	env.=C:	c:\\code\\cicd\\teamcity\\BuildAgent\\bin
1	B	env.DriverData	C:\\Windows\\System32\\Drivers\\DriverData
1	B	env.ProgramW6432	C:\\Program Files
1	B	env.CommonProgramFiles	C:\\Program Files\\Common Files
1	B	env.TEAMCITY_CAPTURE_ENV	"c:\\code\\cicd\\teamcity\\BuildAgent\\jre\\bin\\java.exe" -jar "C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\environment-fetcher\\bin\\env-fetcher.jar"
1	B	env.CommonProgramFiles(x86)	C:\\Program Files (x86)\\Common Files
1	B	env.PUBLIC	C:\\Users\\Public
1	B	env.PATHEXT	.COM;.EXE;.BAT;.CMD;.VBS;.VBE;.JS;.JSE;.WSF;.WSH;.MSC
1	B	env.COMPUTERNAME	RAINBOWHEART
1	B	env.WRAPPER_FILE_SEPARATOR	\\
1	B	system.teamcity.dotnet.nunitlauncher1.1	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\dotnetPlugin\\bin\\JetBrains.BuildServer.NUnitLauncher1.1.exe
1	B	env.APPDATA	C:\\WINDOWS\\system32\\config\\systemprofile\\AppData\\Roaming
1	B	env.SystemDrive	C:
1	B	env.ProgramData	C:\\ProgramData
1	B	env.JDK_1_8_x64	C:\\Program Files\\Java\\jdk1.8.0_131
1	B	system.teamcity.build.tempDir	C:\\code\\cicd\\teamcity\\BuildAgent\\temp\\buildTmp
1	B	env.USERNAME	RAINBOWHEART$
1	B	env.PROCESSOR_LEVEL	23
1	B	env.TMP	C:\\WINDOWS\\TEMP
1	B	env.WRAPPER_OS	windows
1	B	system.agent.home.dir	C:\\code\\cicd\\teamcity\\BuildAgent
1	B	env.ProgramFiles(x86)	C:\\Program Files (x86)
1	B	env.WRAPPER_ARCH	x86
1	B	env.JDK_HOME	C:\\Program Files (x86)\\Java\\jdk1.8.0_131
1	B	env.TEMP	C:\\WINDOWS\\TEMP
1	B	system.agent.name	Default Agent
1	C	teamcity.agent.os.arch.bits	64
1	C	MSBuildTools2.0_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v2.0.50727
1	C	DotNetCoreSDK7.0_Path	C:\\Program Files\\dotnet\\sdk\\7.0.401
1	C	MSBuildTools17.0_x64_Path	C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\amd64
1	C	DotNetCoreSDK8.0_Path	C:\\Program Files\\dotnet\\sdk\\8.0.100-rc.1.23463.5
1	C	DotNetFramework3.5.30729.4926_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v3.5
1	C	teamcity.tool.gant	C:\\code\\cicd\\teamcity\\BuildAgent\\tools\\gant
1	C	DotNetFramework3.0.30729.4926_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v3.0
1	C	teamcity.agent.jvm.file.separator	\\
1	C	teamcity.agent.jvm.user.name	RAINBOWHEART$
1	C	teamcity.agent.jvm.vendor	Amazon.com Inc.
1	C	DotNetFramework2.0.50727_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v2.0.50727
1	C	teamcity.agent.protocol	polling
1	C	teamcity.dotnet.msbuild.extensions2.0	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\dotnetPlugin\\bin\\JetBrains.BuildServer.MSBuildLoggers.dll
1	C	powershell_Desktop_5.1.22621.1_x64_Path	C:\\Windows\\System32\\WindowsPowerShell\\v1.0
1	C	DotNetCoreSDK2.0_Path	C:\\Program Files\\dotnet\\sdk\\2.0.2
1	C	DotNetFramework3.0_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v3.0
1	C	DotNetFramework3.5_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v3.5
1	C	teamcity.agent.ownPort	9090
1	C	powershell_Desktop_5.1.22621.1_x64	5.1.22621.1
1	C	teamcity.dotnet.vstest.15.0	C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe
1	C	DotNetFramework2.0_x64	2.0.50727
1	C	VS2017_Path	C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE
1	C	teamcity.agent.launcher.version	147512
1	C	teamcity.agent.tools.dir	C:\\code\\cicd\\teamcity\\BuildAgent\\tools
1	C	DotNetCoreRuntime6.0.22_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\6.0.22
1	C	VS2022_Path	C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE
1	C	powershell_x64_Path	C:\\WINDOWS\\System32\\WindowsPowerShell\\v1.0
1	C	DotNetCredentialProvider1.0.0_Path	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\nuget-agent\\bin\\credential-plugin\\netcoreapp1.0\\CredentialProvider.TeamCity.dll
1	C	powershell_Desktop_5.1.22621.2506_x64_Path	C:\\WINDOWS\\System32\\WindowsPowerShell\\v1.0
1	C	MSBuildTools15.0_x86_Path	C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Professional\\MSBuild\\15.0\\Bin
1	C	teamcity.agent.jvm.java.home	c:\\code\\cicd\\teamcity\\BuildAgent\\jre
1	C	DotNetCredentialProvider6.0.0_Path	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\nuget-agent\\bin\\credential-plugin\\net6.0\\CredentialProvider.TeamCity.dll
1	C	docker.server.osType	linux
1	C	teamcity.tool.ant-net-tasks	C:\\code\\cicd\\teamcity\\BuildAgent\\tools\\ant-net-tasks
1	C	DotNetFrameworkSDK4.0_x86_Path	C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v10.0A\\bin\\NETFX 4.8 Tools
1	C	DotNetFrameworkTargetingPack3.5_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\v3.5
1	C	teamcity.agent.hardware.cpuCount	16
1	C	DotNetFramework2.0_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v2.0.50727
1	C	DotNetFrameworkTargetingPack4.6_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6
1	C	DotNetFrameworkTargetingPack4.0_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0
1	C	DotNetFramework2.0_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v2.0.50727
1	C	DotNetFrameworkTargetingPack3.0_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\v3.0
1	C	VS2017	15.4.1
1	C	DotNetFramework3.0_x64	3.0.30729.4926
1	C	DotNetFramework4.8_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319
1	C	DotNetCredentialProvider5.0.0_Path	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\nuget-agent\\bin\\credential-plugin\\net5.0\\CredentialProvider.TeamCity.dll
1	C	WindowsSDKv10.0_Path	C:\\Program Files (x86)\\Windows Kits\\10
1	C	container.engine	docker
1	C	powershell_x86	5.1.22621.1
1	C	teamcity.serverUrl	http://localhost:8111
1	C	VS2022	17.7.4
1	C	dockerCompose.version	2.21.0
1	C	DotNetFramework3.5_x86	3.5.30729.4926
1	C	DotNetFramework4.8_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319
1	C	teamcity.agent.jvm.user.timezone	Australia/Sydney
1	C	teamcity.tool.dotCover	%teamcity.tool.JetBrains.dotCover.CommandLineTools.bundled%
1	C	docker.server.version	24.0.6
1	C	DotNetFrameworkTargetingPack4.6.1_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.1
1	C	powershell_x64_Edition	Desktop
1	C	teamcity.agent.jvm.specification	17
1	C	DotNetFramework2.0_x86	2.0.50727
1	C	teamcity.agent.jvm.os.version	10.0
1	C	DotNetFrameworkTargetingPack4.X_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.X
1	C	teamcity.dotnet.mstest.17.0	C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE\\MSTest.exe
1	C	DotNetCoreSDK2.0.2_Path	C:\\Program Files\\dotnet\\sdk\\2.0.2
1	C	DotNetCoreSDK7.0.401_Path	C:\\Program Files\\dotnet\\sdk\\7.0.401
1	C	teamcity.agent.jvm.user.language	en
1	C	powershell_Desktop_x86	5.1.22621.1
1	C	teamcity.agent.name	Default Agent
1	C	DotNetCredentialProvider3.0.0_Path	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\nuget-agent\\bin\\credential-plugin\\netcoreapp3.0\\CredentialProvider.TeamCity.dll
1	C	DotNetCoreRuntime2.0_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\2.0.0
1	C	DotNetFrameworkSDK3.5_x64	8.0.50727
1	C	teamcity.agent.work.dir	C:\\code\\cicd\\teamcity\\BuildAgent\\work
1	C	powershell_x86_Executable	powershell.exe
1	C	DotNetFramework3.0_x86	3.0.30729.4926
1	C	DotNetFramework3.0_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v3.0
1	C	DotNetFramework3.5.30729.4926_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v3.5
1	C	DotNetCoreRuntime6.0_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\6.0.22
1	C	DotNetCoreRuntime7.0_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\7.0.11
1	C	docker.version	24.0.6
1	C	powershell_x64	5.1.22621.2506
1	C	DotNetCoreRuntime8.0_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0-rc.1.23419.4
1	C	teamcity.git.ssh.version	OpenSSH_for_Windows_8.6p
1	C	powershell_Desktop_5.1.22621.1_x86_Path	C:\\Windows\\SysWOW64\\WindowsPowerShell\\v1.0
1	C	teamcity.dotCover.home	%teamcity.tool.JetBrains.dotCover.CommandLineTools.bundled%
1	C	teamcity.agent.jvm.os.name	Windows 11
1	C	DotNetFramework3.5_x64	3.5.30729.4926
1	C	MSBuildTools17.0_x86_Path	C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin
1	C	teamcity.dotnet.msbuild.extensions4.0	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\dotnetPlugin\\bin\\JetBrains.BuildServer.MSBuildLoggers.4.0.dll
1	C	MSBuildTools3.5_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v3.5
1	C	teamcity.agent.hostname	RainbowHeart
1	C	DotNetFramework2.0.50727_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v2.0.50727
1	C	DotNetFrameworkSDK4.0_x64	4.8.03928
1	C	powershell_x86_Path	C:\\Windows\\SysWOW64\\WindowsPowerShell\\v1.0
1	C	teamcity.dotnet.vstest.17.0	C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe
1	C	DotNetFramework3.5_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v3.5
1	C	teamcity.agent.jvm.os.arch	amd64
1	C	teamcity.gitLfs.version	3.4.0
1	C	MSBuildTools15.0_x64_Path	C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Professional\\MSBuild\\15.0\\Bin\\amd64
1	C	DotNetCoreRuntime3.1.32_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\3.1.32
1	C	DotNetFramework4.8_x86	4.8.09032
1	C	DotNetFrameworkTargetingPack4.5.2_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5.2
1	C	DotNetFrameworkTargetingPack4.7.2_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.7.2
1	C	MSBuildTools2.0_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v2.0.50727
1	C	powershell_Desktop_x64	5.1.22621.2506
1	C	DotNetCoreRuntime2.0.0_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\2.0.0
1	C	teamcity.agent.hardware.memorySizeMb	65483
1	C	DotNetFramework3.0.30729.4926_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v3.0
1	C	teamcity.agent.jvm.path.separator	;
1	C	teamcity.agent.jvm.user.home	C:\\WINDOWS\\system32\\config\\systemprofile
1	C	DotNetCoreRuntime3.1_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\3.1.32
1	C	podman.osType	linux
1	C	DotNetFrameworkSDK3.5_x86_Path	C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\bin
1	C	DotNetFrameworkSDK3.5_x86	8.0.50727
1	C	DotNetCredentialProvider2.0.0_Path	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\nuget-agent\\bin\\credential-plugin\\netcoreapp2.0\\CredentialProvider.TeamCity.dll
1	C	DotNetCLI_Path	C:\\Program Files\\dotnet\\dotnet.exe
1	C	DotNetFrameworkSDK3.5_x64_Path	C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\bin\\x64
1	C	DotNetCoreRuntime7.0.11_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\7.0.11
1	C	teamcity.agent.jvm.file.encoding	Cp1252
1	C	DotNetCLI	8.0.100-rc.1.23463.5
1	C	DotNetFrameworkSDK4.0_x64_Path	C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v10.0A\\bin\\NETFX 4.8 Tools\\x64
1	C	DotNetFrameworkTargetingPack4.5.1_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5.1
1	C	DotNetFramework4.8.09032_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319
1	C	DotNetCoreSDK8.0.100-rc.1.23463.5_Path	C:\\Program Files\\dotnet\\sdk\\8.0.100-rc.1.23463.5
1	C	DotNetFrameworkTargetingPack2.0_Path	C:\\Windows\\Microsoft.NET\\Framework\\v2.0.50727
1	C	container.engine.osType	linux
1	C	DotNetFrameworkSDK4.0_x86	4.8.03928
1	C	powershell_x64_Executable	powershell.exe
1	C	WindowsSDKv10.0	10.0.22621
1	C	powershell_Desktop_5.1.22621.1_x86	5.1.22621.1
1	C	DotNetFramework4.8.09032_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319
1	C	teamcity.agent.home.dir	C:\\code\\cicd\\teamcity\\BuildAgent
1	C	MSBuildTools4.0_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319
1	C	DotNetCoreRuntime8.0.0-rc.1.23419.4_Path	C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\8.0.0-rc.1.23419.4
1	C	DotNetCredentialProvider4.0.0_Path	C:\\code\\cicd\\teamcity\\BuildAgent\\plugins\\nuget-agent\\bin\\credential-plugin\\net46\\CredentialProvider.TeamCity.exe
1	C	DotNetFramework4.8_x64	4.8.09032
1	C	teamcity.agent.jvm.version	17.0.7
1	C	MSBuildTools4.0_x86_Path	C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319
1	C	teamcity.tool.jps	C:\\code\\cicd\\teamcity\\BuildAgent\\tools\\jps
1	C	MSBuildTools3.5_x64_Path	C:\\Windows\\Microsoft.NET\\Framework64\\v3.5
1	C	teamcity.dotnet.mstest.15.0	C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\MSTest.exe
1	C	teamcity.agent.jvm.user.country	GB
1	C	DotNetFrameworkTargetingPack4.8_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.8
1	C	powershell_Desktop_5.1.22621.2506_x64	5.1.22621.2506
1	C	powershell_x86_Edition	Desktop
1	C	DotNetFrameworkTargetingPack4.5_Path	C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5
1	C	teamcity.tool.NuGet.CommandLine.6.8.0	C:\\code\\cicd\\teamcity\\BuildAgent\\tools\\NuGet.CommandLine.6.8.0
1	B	env.Path	C:\\Program Files\\Microsoft MPI\\Bin\\;C:\\Windows\\system32;C:\\Windows;C:\\Windows\\System32\\Wbem;C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\;C:\\Windows\\System32\\OpenSSH\\;C:\\Program Files (x86)\\NVIDIA Corporation\\PhysX\\Common;C:\\Program Files\\dotnet\\;C:\\Program Files\\Microsoft SQL Server\\130\\Tools\\Binn\\;C:\\Program Files (x86)\\GtkSharp\\2.12\\bin;C:\\WINDOWS\\system32;C:\\WINDOWS;C:\\WINDOWS\\System32\\Wbem;C:\\WINDOWS\\System32\\WindowsPowerShell\\v1.0\\;C:\\WINDOWS\\System32\\OpenSSH\\;C:\\Program Files\\Microsoft SQL Server\\150\\Tools\\Binn\\;C:\\Program Files\\Microsoft SQL Server\\Client SDK\\ODBC\\170\\Tools\\Binn\\;C:\\Program Files\\Git\\cmd;C:\\Program Files\\Docker\\Docker\\resources\\bin;C:\\Program Files (x86)\\Gpg4win\\..\\GnuPG\\bin;C:\\Program Files\\NVIDIA Corporation\\NVIDIA NvDLISR;C:\\WINDOWS\\system32\\config\\systemprofile\\AppData\\Local\\Microsoft\\WindowsApps;C:\\WINDOWS\\system32\\config\\systemprofile\\.dotnet\\tools
1	B	system.teamcity.agent.cpuBenchmark	934
1	C	teamcity.agent.work.dir.freeSpaceMb	227056
\.


--
-- TOC entry 6288 (class 0 OID 16646)
-- Dependencies: 250
-- Data for Name: agent_type_runner; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.agent_type_runner (agent_type_id, runner) FROM stdin;
1	NUnit
1	dotnet
1	Ant
1	jetbrains.mspec
1	jetbrains.dotNetGenericRunner
1	JPS
1	kotlinScript
1	MSBuild
1	python-runner
1	ssh-deploy-runner
1	Maven2
1	jetbrains_powershell
1	NAnt
1	VS.Solution
1	FxCop
1	ssh-exec-runner
1	dotnet-tools-inspectcode
1	VisualStudioTest
1	jb.nuget.pack
1	smb-deploy-runner
1	SBT
1	Duplicator
1	cargo-deploy-runner
1	Inspection
1	sln2003
1	Qodana
1	csharpScript
1	dotnet-tools-dupfinder
1	jb.nuget.publish
1	gradle-runner
1	simpleRunner
1	rake-runner
1	jb.nuget.installer
1	DockerCommand
1	nodejs-runner
1	ftp-deploy-runner
\.


--
-- TOC entry 6289 (class 0 OID 16651)
-- Dependencies: 251
-- Data for Name: agent_type_vcs; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.agent_type_vcs (agent_type_id, vcs) FROM stdin;
1	tfs
1	jetbrains.git
1	mercurial
1	svn
1	perforce
\.


--
-- TOC entry 6400 (class 0 OID 17417)
-- Dependencies: 362
-- Data for Name: audit_additional_object; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.audit_additional_object (comment_id, object_index, object_id, object_name) FROM stdin;
5	0	buildType:bt1|1|2	version before: 1, version after: 2
6	0	buildType:bt1|2|3	version before: 2, version after: 3
8	0	_Root	"<Root project>"
9	0	project:project1|1|2	version before: 1, version after: 2
12	0	NO_ID	Fortitude_HttpsGithubComShwaindogFortitudeRefsHeadsMain
12	1	NO_ID	Fortitude_SshGitGithubComShwaindogFortitudeGitRefsHeadsMain
13	0	buildType:bt1|3|4	version before: 3, version after: 4
14	0	vcsRoot:1|1|2	version before: 1, version after: 2
101	0	NO_ID	teamcity-commit-hooks
102	0	NO_ID	teamcity-commit-hooks
103	0	buildType:bt1|4|5	version before: 4, version after: 5
104	0	buildType:bt1|5|6	version before: 5, version after: 6
201	0	buildType:bt1|6|7	version before: 6, version after: 7
202	0	buildType:bt1|7|8	version before: 7, version after: 8
\.


--
-- TOC entry 6275 (class 0 OID 16566)
-- Dependencies: 237
-- Data for Name: backup_builds; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.backup_builds (build_id) FROM stdin;
\.


--
-- TOC entry 6274 (class 0 OID 16558)
-- Dependencies: 236
-- Data for Name: backup_info; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.backup_info (mproc_id, file_name, file_size, started, finished, status) FROM stdin;
\.


--
-- TOC entry 6349 (class 0 OID 17093)
-- Dependencies: 311
-- Data for Name: build_artifact_dependency; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_artifact_dependency (artif_dep_id, build_state_id, source_build_type_id, revision_rule, branch, src_paths) FROM stdin;
\.


--
-- TOC entry 6338 (class 0 OID 17013)
-- Dependencies: 300
-- Data for Name: build_attrs; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_attrs (build_state_id, attr_name, attr_value, attr_num_value) FROM stdin;
1	teamcity.build.creatorNodeId	MAIN_SERVER	\N
1	teamcity.build.serverBuildNumber	147512	\N
1	teamcity.build.buildTypeArtifactsPath	Fortitude/Build	\N
1	teamcity.build.desiredBranchName		\N
1	teamcity.build.hasFrozenSettings	current	\N
1	teamcity.checkout.properties.hash	e5524d836dae67fd	\N
1	teamcity.build.finalBranchName		\N
1	teamcity.upperLimitRevision.1	67c6d2dfc0b7a225f5930f6c2fb05d99d4a70b0f	\N
1	teamcity.build.branchDisplayName	main	\N
1	teamcity.build.agentAccessCode	Z4QQAmSnQ4hqoCwzzmZxlmNC1DqfBYsI	\N
1	teamcity.build.settingsDigest	094904196238e2b62bb185bb4237cd4191391908	\N
1	teamcity.build.ownerNodeUrl	#	\N
101	teamcity.build.creatorNodeId	MAIN_SERVER	\N
101	teamcity.build.serverBuildNumber	147512	\N
101	teamcity.build.buildTypeArtifactsPath	Fortitude/Build	\N
101	teamcity.build.desiredBranchName	feature/AddTeamcityBuildToCicd	\N
101	teamcity.build.hasFrozenSettings	current	\N
101	teamcity.checkout.properties.hash	fbf683effb5875b6	\N
101	teamcity.build.finalBranchName	feature/AddTeamcityBuildToCicd	\N
101	teamcity.upperLimitRevision.2	cc625b80fa8d68a300397d498553649485c70862	\N
101	teamcity.build.agentAccessCode	vtzIGiubEYD4fqkwJc9pw2OM7YCp294Q	\N
101	teamcity.build.settingsDigest	d582129887cca8c77b01bbb3729b92c77f20242a	\N
101	teamcity.build.ownerNodeUrl	#	\N
102	teamcity.build.creatorNodeId	MAIN_SERVER	\N
102	teamcity.build.serverBuildNumber	147512	\N
102	teamcity.build.buildTypeArtifactsPath	Fortitude/Build	\N
102	teamcity.build.desiredBranchName	feature/AddTeamcityBuildToCicd	\N
102	teamcity.build.hasFrozenSettings	current	\N
102	teamcity.checkout.properties.hash	fbf683effb5875b6	\N
102	teamcity.build.finalBranchName	feature/AddTeamcityBuildToCicd	\N
102	teamcity.upperLimitRevision.2	01be4c770e3295ab944eefdb9510511e7987b74e	\N
102	teamcity.build.agentAccessCode	nw1e6hdDW9GRw6jyRRErnPLu6UrWNAbH	\N
102	teamcity.build.settingsDigest	d582129887cca8c77b01bbb3729b92c77f20242a	\N
102	teamcity.build.ownerNodeUrl	#	\N
\.


--
-- TOC entry 6356 (class 0 OID 17134)
-- Dependencies: 318
-- Data for Name: build_checkout_rules; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_checkout_rules (build_state_id, vcs_root_id, checkout_rules) FROM stdin;
1	1	
101	2	
102	2	
\.


--
-- TOC entry 6339 (class 0 OID 17022)
-- Dependencies: 301
-- Data for Name: build_data_storage; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_data_storage (build_id, metric_id, metric_value) FROM stdin;
1	1320950366645669724	70.000000
1	3151721548560894666	129.000000
1	6971143902851089546	1005.000000
1	-8187224208195643804	69.000000
1	-2867214296632653833	178954.000000
1	6462680959755247356	272.000000
1	1662119473372385625	10392.000000
1	4967928924983341179	24.000000
1	-1666698690275047195	36223.000000
1	7480765492827052076	45404.000000
1	-1219941059002544593	7734.000000
1	-3811876622637497843	2482.000000
1	340349775839283755	500.000000
1	9087962820595322767	35.000000
1	6662056484837638591	1369.000000
1	-3896958621569208557	1.000000
1	-6371738409877395985	180211.000000
1	3014510524022395756	1249247.000000
1	5744866293795496522	1369.000000
1	-4924562750898875443	1.000000
101	1320950366645669724	111.000000
101	3151721548560894666	459.000000
101	-8187224208195643804	158.000000
101	6971143902851089546	2530.000000
101	6462680959755247356	11.000000
101	1662119473372385625	21310.000000
101	4967928924983341179	14.000000
101	-1666698690275047195	28518.000000
101	7480765492827052076	63973.000000
101	-1219941059002544593	16203.000000
101	-3811876622637497843	3317.000000
101	340349775839283755	529.000000
101	9087962820595322767	94.000000
101	6662056484837638591	1369.000000
101	-3896958621569208557	1.000000
101	-6371738409877395985	3154.000000
101	3014510524022395756	1278882.000000
101	5744866293795496522	1369.000000
101	-4924562750898875443	1.000000
102	1320950366645669724	2.000000
102	3151721548560894666	308.000000
102	-8187224208195643804	58.000000
102	6971143902851089546	2508.000000
102	6462680959755247356	1.000000
102	1662119473372385625	8176.000000
102	4967928924983341179	4.000000
102	-1666698690275047195	22256.000000
102	7480765492827052076	56685.000000
102	-1219941059002544593	14535.000000
102	-3811876622637497843	1585.000000
102	340349775839283755	575.000000
102	9087962820595322767	91.000000
102	6662056484837638591	1369.000000
102	-3896958621569208557	1.000000
102	-6371738409877395985	2844.000000
102	3014510524022395756	1233627.000000
102	5744866293795496522	1369.000000
102	-4924562750898875443	1.000000
\.


--
-- TOC entry 6337 (class 0 OID 17007)
-- Dependencies: 299
-- Data for Name: build_dependency; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_dependency (build_state_id, depends_on, dependency_options) FROM stdin;
\.


--
-- TOC entry 6378 (class 0 OID 17275)
-- Dependencies: 340
-- Data for Name: build_labels; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_labels (build_id, vcs_root_id, label, status, error_message) FROM stdin;
\.


--
-- TOC entry 6383 (class 0 OID 17311)
-- Dependencies: 345
-- Data for Name: build_overriden_roots; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_overriden_roots (build_state_id, original_vcs_root_id, substitution_vcs_root_id) FROM stdin;
\.


--
-- TOC entry 6347 (class 0 OID 17077)
-- Dependencies: 309
-- Data for Name: build_problem; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_problem (build_state_id, problem_id, problem_description) FROM stdin;
\.


--
-- TOC entry 6348 (class 0 OID 17085)
-- Dependencies: 310
-- Data for Name: build_problem_attribute; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_problem_attribute (build_state_id, problem_id, attr_name, attr_value) FROM stdin;
\.


--
-- TOC entry 6365 (class 0 OID 17192)
-- Dependencies: 327
-- Data for Name: build_problem_muted; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_problem_muted (build_state_id, problem_id, mute_id) FROM stdin;
\.


--
-- TOC entry 6336 (class 0 OID 17001)
-- Dependencies: 298
-- Data for Name: build_project; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_project (build_id, project_level, project_int_id) FROM stdin;
1	0	_Root
1	1	project1
101	0	_Root
101	1	project1
102	0	_Root
102	1	project1
\.


--
-- TOC entry 6368 (class 0 OID 17210)
-- Dependencies: 330
-- Data for Name: build_queue; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_queue (build_type_id, agent_restrictor_type_id, agent_restrictor_id, requestor, build_state_id) FROM stdin;
\.


--
-- TOC entry 6369 (class 0 OID 17216)
-- Dependencies: 331
-- Data for Name: build_queue_order; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_queue_order (version, line_num, promotion_ids) FROM stdin;
3	1	102
\.


--
-- TOC entry 6375 (class 0 OID 17254)
-- Dependencies: 337
-- Data for Name: build_revisions; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_revisions (build_state_id, vcs_root_id, vcs_revision, vcs_revision_display_name, vcs_branch_name, modification_id, vcs_root_type, checkout_mode) FROM stdin;
1	1	67c6d2dfc0b7a225f5930f6c2fb05d99d4a70b0f	67c6d2dfc0b7a225f5930f6c2fb05d99d4a70b0f	refs/heads/main	\N	\N	\N
101	2	cc625b80fa8d68a300397d498553649485c70862	cc625b80fa8d68a300397d498553649485c70862	refs/heads/feature/AddTeamcityBuildToCicd	1	\N	\N
102	2	01be4c770e3295ab944eefdb9510511e7987b74e	01be4c770e3295ab944eefdb9510511e7987b74e	refs/heads/feature/AddTeamcityBuildToCicd	2	\N	\N
\.


--
-- TOC entry 6401 (class 0 OID 17423)
-- Dependencies: 363
-- Data for Name: build_set_tmp; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_set_tmp (build_id) FROM stdin;
\.


--
-- TOC entry 6332 (class 0 OID 16949)
-- Dependencies: 294
-- Data for Name: build_state; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_state (id, build_id, build_type_id, modification_id, chain_modification_id, personal_modification_id, personal_user_id, is_personal, is_canceled, is_changes_detached, is_deleted, branch_name, queued_time, remove_from_queue_time) FROM stdin;
1	1	bt1	-1	-1	\N	\N	0	0	0	0	\N	1707128434888	1707128615099
101	101	bt1	1	1	\N	\N	0	0	0	0	feature/AddTeamcityBuildToCicd	1710285436744	1710285439898
102	102	bt1	2	2	\N	\N	0	0	0	0	feature/AddTeamcityBuildToCicd	1710289251694	1710289254538
\.


--
-- TOC entry 6382 (class 0 OID 17305)
-- Dependencies: 344
-- Data for Name: build_state_private_tag; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_state_private_tag (build_state_id, owner, tag) FROM stdin;
1	1	.teamcity.star
\.


--
-- TOC entry 6381 (class 0 OID 17299)
-- Dependencies: 343
-- Data for Name: build_state_tag; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_state_tag (build_state_id, tag) FROM stdin;
\.


--
-- TOC entry 6279 (class 0 OID 16588)
-- Dependencies: 241
-- Data for Name: build_type; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_type (int_id, config_id, origin_project_id, delete_time) FROM stdin;
bt1	9b4c7f25-6b73-4764-99ab-cf2c7ecf6ba0	\N	\N
\.


--
-- TOC entry 6284 (class 0 OID 16623)
-- Dependencies: 246
-- Data for Name: build_type_counters; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_type_counters (build_type_id, counter) FROM stdin;
bt1	4
\.


--
-- TOC entry 6352 (class 0 OID 17112)
-- Dependencies: 314
-- Data for Name: build_type_edge_relation; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_type_edge_relation (child_modification_id, build_type_id, parent_num, change_type) FROM stdin;
\.


--
-- TOC entry 6355 (class 0 OID 17129)
-- Dependencies: 317
-- Data for Name: build_type_group_vcs_change; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_type_group_vcs_change (modification_id, group_id, change_type) FROM stdin;
1	1	\N
2	1	\N
\.


--
-- TOC entry 6282 (class 0 OID 16609)
-- Dependencies: 244
-- Data for Name: build_type_mapping; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_type_mapping (int_id, ext_id, main) FROM stdin;
bt1	Fortitude_Build	1
\.


--
-- TOC entry 6351 (class 0 OID 17106)
-- Dependencies: 313
-- Data for Name: build_type_vcs_change; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.build_type_vcs_change (modification_id, build_type_id, change_type) FROM stdin;
\.


--
-- TOC entry 6340 (class 0 OID 17027)
-- Dependencies: 302
-- Data for Name: canceled_info; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.canceled_info (build_id, user_id, description, interrupt_type) FROM stdin;
\.


--
-- TOC entry 6402 (class 0 OID 17428)
-- Dependencies: 364
-- Data for Name: clean_checkout_enforcement; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.clean_checkout_enforcement (build_type_id, agent_id, current_build_id, request_time) FROM stdin;
\.


--
-- TOC entry 6276 (class 0 OID 16571)
-- Dependencies: 238
-- Data for Name: cleanup_history; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.cleanup_history (proc_id, start_time, finish_time, interrupt_reason) FROM stdin;
1	1708830000025	1708830000905	\N
2	1709268356275	1709268357028	\N
3	1709456256167	1709456256920	\N
4	1709723841416	1709723842104	\N
5	1709809150620	1709809151344	\N
6	1710020804952	1710020805766	\N
7	1710039599997	1710039600768	\N
11	1710125999985	1710126000973	\N
\.


--
-- TOC entry 6292 (class 0 OID 16675)
-- Dependencies: 254
-- Data for Name: cloud_image_state; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.cloud_image_state (project_id, profile_id, image_id, name) FROM stdin;
\.


--
-- TOC entry 6296 (class 0 OID 16697)
-- Dependencies: 258
-- Data for Name: cloud_image_without_agent; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.cloud_image_without_agent (profile_id, cloud_code, image_id, last_update) FROM stdin;
\.


--
-- TOC entry 6293 (class 0 OID 16680)
-- Dependencies: 255
-- Data for Name: cloud_instance_state; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.cloud_instance_state (project_id, profile_id, image_id, instance_id, name, last_update, status, start_time, network_identity, is_expired, agent_id) FROM stdin;
\.


--
-- TOC entry 6295 (class 0 OID 16692)
-- Dependencies: 257
-- Data for Name: cloud_started_instance; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.cloud_started_instance (profile_id, cloud_code, image_id, instance_id, last_update) FROM stdin;
\.


--
-- TOC entry 6294 (class 0 OID 16685)
-- Dependencies: 256
-- Data for Name: cloud_state_data; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.cloud_state_data (project_id, profile_id, image_id, instance_id, data) FROM stdin;
\.


--
-- TOC entry 6398 (class 0 OID 17403)
-- Dependencies: 360
-- Data for Name: comments; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.comments (id, author_id, when_changed, commentary) FROM stdin;
1	1	1706960407354	
2	1	1706960701691	
3	1	1706960701728	In project: Fortitude
4	1	1706960701805	
5	1	1706960749516	3 build step(s) were added to 'Build' build configuration
6	1	1706960781578	runners of 'Build' build configuration were updated
7	1	1706961313886	Downloaded
8	1	1706961313912	
9	1	1706961354819	project settings were updated
10	1	1707128434933	
11	1	1707128585084	Windows TeamCity Build
12	1	1707130748571	
13	1	1707130748584	'https://github.com/shwaindog/Fortitude#refs/heads/main' VCS root was updated
14	1	1707130748607	'https://github.com/shwaindog/Fortitude#refs/heads/main' VCS root was updated
101	1	1710031421464	
102	1	1710031428386	
103	1	1710033750253	general settings of 'Build' build configuration were updated
104	1	1710033784102	parameters of 'Build' build configuration were updated
201	1	1710289346007	general settings of 'Build' build configuration were updated
202	1	1710289378877	general settings of 'Build' build configuration were updated
\.


--
-- TOC entry 6372 (class 0 OID 17235)
-- Dependencies: 334
-- Data for Name: compiler_output; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.compiler_output (build_id, message_order, message) FROM stdin;
\.


--
-- TOC entry 6412 (class 0 OID 17493)
-- Dependencies: 374
-- Data for Name: config_persisting_tasks; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.config_persisting_tasks (id, task_type, description, stage, node_id, created, updated) FROM stdin;
36	project_configs	general settings of 'Build' build configuration were updated	5	MAIN_SERVER	1710289345635	1710289346045
37	project_configs	Storing 1 events into the multi-node events table	5	MAIN_SERVER	1710289357463	1710289357677
38	project_configs	general settings of 'Build' build configuration were updated	5	MAIN_SERVER	1710289378571	1710289378904
39	project_configs	Storing 1 events into the multi-node events table	5	MAIN_SERVER	1710289390476	1710289390684
35	global_settings	Save Priority classes	5	MAIN_SERVER	1710126000115	1710126001169
\.


--
-- TOC entry 6409 (class 0 OID 17474)
-- Dependencies: 371
-- Data for Name: custom_data; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.custom_data (data_key_hash, collision_idx, data_domain, data_key, data_id) FROM stdin;
f0cb7c07f07e4122d0537af1e76556a717a8bd21	0	buildType:bt1	storage:diskusage:ae6a8165-e9f5-4301-b0d5-164d6f24e831	1
d02a2986a1e963c11625737496b0e67b2b32ec2d	0	vcsRoot:1	1:state	2
8820b248ec73b2bfc7944ccf6f93a938e311f12a	0	buildType:bt1	jetbrains.buildServer.buildTriggers.vcs.VcsBuildTriggerService_TRIGGER_1	3
420d2033c1131954b71b62f7f5fe9a3865545cdf	0	buildType:bt1	jetbrains.buildServer.buildTriggers.vcs.VcsRootChangesLoader	5
9368067c638131a8cec6ee59cd2a999f0067a63e	0	vcsRoot:1	1:pollingState	6
068e78130440b4a734776bba7f8ed43afcff1875	0	vcsRoot:1	1:stateDates	7
2569d7e3b10171263be901f16ae30b6887786ca0	0	buildType:bt1	disk:usage:cleaned:builds	9
17c6e30bba0f765fa1aa540b124f2ad5b1319ee8	0	vcsRoot:1	2:state	10
003c712c46e2e2065147a35795d590f871dc8e51	0	vcsRoot:1	2:pollingState	12
1210df620622460589bd94f382f9e3aa632d37d8	0	java.lang.Class	jetbrains.buildServer.serverSide.impl.cleanup.ServerCleanupState	13
cb688074bda43d036c553a5c74fd0182e0fdd3eb	0	vcsRoot:1	2:stateDates	24
\.


--
-- TOC entry 6408 (class 0 OID 17466)
-- Dependencies: 370
-- Data for Name: custom_data_body; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.custom_data_body (id, part_num, total_parts, data_body, update_date) FROM stdin;
6	0	1	lastCheckingForChangesTime=1707130744230\n	1707130744232
5	0	1	prevBranchSpec-1=+:refs/heads/*\\-\nprevRootInstance-1=2\n	1707130767450
24	0	1		1710289234155
10	0	1	##teamcity##:createTime=1710289225999\n##teamcity##:defaultBranch=refs/heads/main\n##teamcity##:lastUpdatedBy=MAIN_SERVER\nrefs/heads/feature/AddTeamcityBuildToCicd=01be4c770e3295ab944eefdb9510511e7987b74e\nrefs/heads/main=67c6d2dfc0b7a225f5930f6c2fb05d99d4a70b0f\n	1710289234164
3	0	1	lastProcessedModId-<default>=-1\nlastProcessedModId-feature/AddTeamcityBuildToCicd=2-2\nlastTriggeredBy-<default>=-1:118770\nlastTriggeredBy-feature/AddTeamcityBuildToCicd=2-2:118770\nteamcityPrevCallTimestamp=1710060900271\nteamcityTriggerStatePropsHash=sha1:ac636728e4810782e4fd210eeb8eca147ed7b5bf\n	1710289311883
9	0	1	cleaned_number=3\n	1710289431923
1	0	1	diskusagedata={"ver":3,"log":3844484,"art":0,"artI":61759,"non":0,"builds":[[1,1393868],[101,1278815],[102,1233560]],"totBuilds":3,"cleanBuilds":3,"cleanSize":0,"date":-1}\n	1710289431928
2	0	1	##teamcity##:createTime=1707127497233\n##teamcity##:defaultBranch=refs/heads/main\n##teamcity##:lastUpdatedBy=MAIN_SERVER\nrefs/heads/main=67c6d2dfc0b7a225f5930f6c2fb05d99d4a70b0f\n	1707127497235
7	0	1		1707113094486
12	0	1	lastCheckingForChangesTime=1710290027835\n	1710290027836
13	0	1	buildsToProcess=0\ncurrentStage=Clean-up finished\nfinishTimeKey=1710126000973\nprocessedBuilds=-1\nstartTimeKey=1710125999985\n	1710126000985
\.


--
-- TOC entry 6316 (class 0 OID 16837)
-- Dependencies: 278
-- Data for Name: data_storage_dict; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.data_storage_dict (metric_id, value_type_key) FROM stdin;
-1709115199521145549	queueWaitReason:CVPThere_are_no_idle_compatible_agents_which_can_run_this_build
1320950366645669724	queueWaitReason:Waiting_for_the_build_queue_distribution_process
3151721548560894666	queueWaitReason:Waiting_to_start_checking_for_changes
6971143902851089546	queueWaitReason:Checking_for_changes_is_in_progress
-8558628221356790617	queueWaitReason:CVPPreparing_a_build_for_a_start_on_an_agent
-4749060626145661217	queueWaitReason:CVPWaiting_for_the_build_queue_distribution_process
2002028620177770854	queueWaitReason:CVPWaiting_to_start_checking_for_changes
7828127420197389409	queueWaitReason:CVPChecking_for_changes_is_in_progress
-8187224208195643804	queueWaitReason:Preparing_a_build_for_a_start_on_an_agent
-2867214296632653833	queueWaitReason:There_are_no_idle_compatible_agents_which_can_run_this_build
6462680959755247356	buildStageDuration:toolsUpdating
-1666698690275047195	buildStageDuration:buildStepdotnet
1662119473372385625	buildStageDuration:sourcesUpdate
4967928924983341179	buildStageDuration:firstStepPreparation
7480765492827052076	buildStageDuration:buildStepdotnet_1
-1219941059002544593	buildStageDuration:buildStepjb_nuget_installer
-3811876622637497843	buildStageDuration:buildFinishing
340349775839283755	buildStageDuration:artifactsPublishing
9087962820595322767	serverSideBuildFinishing
6662056484837638591	TotalTestCount
-3896958621569208557	SuccessRate
-6371738409877395985	TimeSpentInQueue
3014510524022395756	ArtifactsSize
5744866293795496522	PassedTestCount
-4924562750898875443	BuildTestStatus
\.


--
-- TOC entry 6273 (class 0 OID 16551)
-- Dependencies: 235
-- Data for Name: db_heartbeat; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.db_heartbeat (node_id, starting_code, starting_time, lock_mode, ip_address, additional_info, last_time, update_interval, uuid, app_type, url, access_token, build_number, display_version, responsibilities, unix_last_time, unix_starting_time) FROM stdin;
MAIN_SERVER	960742932	\N	W	172.17.0.2	IP: 172.17.0.2, Port: 8111, Installation Directory: /opt/teamcity, Data Directory: /data/teamcity_server/datadir	\N	60000	fd79e95b-daa8-458b-9b4d-fa707dce4130	main_server	\N	\N	147512	2023.11.3	133143986207	1710290027913	1710284864653
\.


--
-- TOC entry 6269 (class 0 OID 16532)
-- Dependencies: 231
-- Data for Name: db_version; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.db_version (version_number, version_time, incompatible_change) FROM stdin;
1003	2024-02-03 11:38:23.429	1
\.


--
-- TOC entry 6376 (class 0 OID 17263)
-- Dependencies: 338
-- Data for Name: default_build_parameters; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.default_build_parameters (build_state_id, param_name, param_value) FROM stdin;
\.


--
-- TOC entry 6277 (class 0 OID 16576)
-- Dependencies: 239
-- Data for Name: domain_sequence; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.domain_sequence (domain_name, last_used_value) FROM stdin;
modification_id	2
user_id	10
audit_comment_id	300
node_task_id	458610
project_int_id	10
vcs_root_int_id	10
bt_int_id	10
server_health_item_id	10
promotion_id	200
agent_id	10
agent_type_id	10
cleanup_proc_id	20
\.


--
-- TOC entry 6374 (class 0 OID 17247)
-- Dependencies: 336
-- Data for Name: downloaded_artifacts; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.downloaded_artifacts (target_build_id, source_build_id, download_timestamp, artifact_path) FROM stdin;
\.


--
-- TOC entry 6394 (class 0 OID 17382)
-- Dependencies: 356
-- Data for Name: duplicate_diff; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.duplicate_diff (build_id, hash) FROM stdin;
\.


--
-- TOC entry 6395 (class 0 OID 17387)
-- Dependencies: 357
-- Data for Name: duplicate_fragments; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.duplicate_fragments (id, file_id, line, offset_info) FROM stdin;
\.


--
-- TOC entry 6393 (class 0 OID 17376)
-- Dependencies: 355
-- Data for Name: duplicate_results; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.duplicate_results (id, build_id, hash, cost) FROM stdin;
\.


--
-- TOC entry 6396 (class 0 OID 17393)
-- Dependencies: 358
-- Data for Name: duplicate_stats; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.duplicate_stats (build_id, total, new_total, old_total) FROM stdin;
\.


--
-- TOC entry 6341 (class 0 OID 17032)
-- Dependencies: 303
-- Data for Name: failed_tests; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.failed_tests (test_name_id, build_id, test_id, ffi_build_id) FROM stdin;
\.


--
-- TOC entry 6371 (class 0 OID 17228)
-- Dependencies: 333
-- Data for Name: failed_tests_output; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.failed_tests_output (build_id, test_id, problem_description, std_output, error_output, stacktrace, expected, actual) FROM stdin;
\.


--
-- TOC entry 6350 (class 0 OID 17099)
-- Dependencies: 312
-- Data for Name: final_artifact_dependency; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.final_artifact_dependency (artif_dep_id, build_state_id, source_build_type_id, revision_rule, branch, src_paths) FROM stdin;
\.


--
-- TOC entry 6411 (class 0 OID 17489)
-- Dependencies: 373
-- Data for Name: hidden_health_item; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.hidden_health_item (item_id, user_id) FROM stdin;
1	1
2	1
\.


--
-- TOC entry 6334 (class 0 OID 16974)
-- Dependencies: 296
-- Data for Name: history; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.history (build_id, agent_name, build_type_id, branch_name, build_start_time_server, build_start_time_agent, build_finish_time_server, remove_from_queue_time, queued_time, status, status_text, user_status_text, pin, is_personal, is_canceled, build_number, requestor, build_state_id, agent_type_id) FROM stdin;
1	Default Agent	bt1	\N	1707128615531	1707128615479	1707128727117	1707128615099	1707128434888	1	Tests passed: 1369	\N	0	0	0	1	##userId='1' type='user'	1	1
101	Default Agent	bt1	feature/AddTeamcityBuildToCicd	1710285440740	1710285440677	1710285582560	1710285439898	1710285436744	1	Tests passed: 1369	\N	0	0	0	0.1.2.cc625b80fa8d68a300397d498553649485c70862	##vcsName='jetbrains.git' type='vcs' triggerId='TRIGGER_1'	101	1
102	Default Agent	bt1	feature/AddTeamcityBuildToCicd	1710289254803	1710289254777	1710289366139	1710289254538	1710289251694	1	Tests passed: 1369	\N	0	0	0	0.1.3.01be4c770e3295ab944eefdb9510511e7987b74e	##vcsName='jetbrains.git' type='vcs' triggerId='TRIGGER_1'	102	1
\.


--
-- TOC entry 6353 (class 0 OID 17118)
-- Dependencies: 315
-- Data for Name: ids_group; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.ids_group (id, group_hash) FROM stdin;
1	2e3df42eaa13ada52be0f5e9c1522bc90d4bd689
\.


--
-- TOC entry 6354 (class 0 OID 17124)
-- Dependencies: 316
-- Data for Name: ids_group_entity_id; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.ids_group_entity_id (group_id, entity_id) FROM stdin;
1	bt1
\.


--
-- TOC entry 6373 (class 0 OID 17241)
-- Dependencies: 335
-- Data for Name: ignored_tests; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.ignored_tests (build_id, test_id, ignore_reason) FROM stdin;
\.


--
-- TOC entry 6387 (class 0 OID 17339)
-- Dependencies: 349
-- Data for Name: inspection_data; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.inspection_data (hash, result, severity, type_pattern, fqname, file_name, parent_fqnames, parent_type_patterns, module_name, inspection_id, is_local, used) FROM stdin;
\.


--
-- TOC entry 6391 (class 0 OID 17363)
-- Dependencies: 353
-- Data for Name: inspection_diff; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.inspection_diff (build_id, hash) FROM stdin;
\.


--
-- TOC entry 6388 (class 0 OID 17349)
-- Dependencies: 350
-- Data for Name: inspection_fixes; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.inspection_fixes (hash, hint) FROM stdin;
\.


--
-- TOC entry 6386 (class 0 OID 17330)
-- Dependencies: 348
-- Data for Name: inspection_info; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.inspection_info (id, inspection_id, inspection_name, inspection_desc, group_name) FROM stdin;
\.


--
-- TOC entry 6389 (class 0 OID 17353)
-- Dependencies: 351
-- Data for Name: inspection_results; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.inspection_results (build_id, hash, line) FROM stdin;
\.


--
-- TOC entry 6390 (class 0 OID 17358)
-- Dependencies: 352
-- Data for Name: inspection_stats; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.inspection_stats (build_id, total, new_total, old_total, errors, new_errors, old_errors) FROM stdin;
\.


--
-- TOC entry 6415 (class 0 OID 17514)
-- Dependencies: 377
-- Data for Name: light_history; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.light_history (build_id, agent_name, build_type_id, build_start_time_server, build_start_time_agent, build_finish_time_server, status, status_text, user_status_text, pin, is_personal, is_canceled, build_number, requestor, queued_time, remove_from_queue_time, build_state_id, agent_type_id, branch_name) FROM stdin;
\.


--
-- TOC entry 6314 (class 0 OID 16822)
-- Dependencies: 276
-- Data for Name: long_file_name; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.long_file_name (hash, file_name) FROM stdin;
\.


--
-- TOC entry 6270 (class 0 OID 16538)
-- Dependencies: 232
-- Data for Name: meta_file_line; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.meta_file_line (file_name, line_nr, line_text) FROM stdin;
backup.config	1	-- INTERNAL BACKUP CONFIGURATION
backup.config	2	
backup.config	3	
backup.config	4	TABLES-NOT-TO-BACKUP
backup.config	5	
backup.config	6	  db_heartbeat
backup.config	7	  build_set_tmp
backup.config	8	  backup_builds
backup.config	9	  node_events
backup.config	10	  node_tasks
backup.config	11	  node_tasks_long_value
backup.config	12	  node_locks
backup.config	13	  cloud_state_data
backup.config	14	  cloud_image_state
backup.config	15	  cloud_instance_state
backup.config	16	  cloud_instance_start
backup.config	17	
backup.config	18	
backup.config	19	TABLES-TO-FILTER-BY-BUILDS
backup.config	20	
backup.config	21	  build_data_storage
backup.config	22	  build_labels
backup.config	23	  compiler_output
backup.config	24	  failed_tests_output
backup.config	25	  failed_tests
backup.config	26	  ignored_tests
backup.config	27	  personal_build_relative_path
backup.config	28	  stats
backup.config	29	  test_info
backup.config	30	  test_info_archive
backup.config	31	  test_metadata
backup.config	32	  inspection_results
backup.config	33	  inspection_stats
backup.config	34	  inspection_diff
backup.config	35	  duplicate_results
backup.config	36	  duplicate_diff
backup.config	37	  duplicate_stats
backup.config	38	  running
backup.config	39	  history
backup.config	40	  removed_builds_history
backup.config	41	  light_history
backup.config	42	
backup.config	43	
backup.config	44	
backup.config	45	
backup.config	46	
backup.config	47	
schema.config	1	-- TEAMCITY DATABASE SCHEMA
schema.config	2	
schema.config	3	
schema.config	4	-- Each table describes as a table name started from the first position of the line,
schema.config	5	-- and followed by its inner constructions with offset. All inner constructions
schema.config	6	-- should be written with the same offset.
schema.config	7	
schema.config	8	
schema.config	9	
schema.config	10	--                              ABBREVIATIONS
schema.config	11	--                              -------------
schema.config	12	--                              M:  mandatory (not null)
schema.config	13	--                              PK: primary key
schema.config	14	--                              AK: alternative key (unique index)
schema.config	15	--                              IE: inversion entry (non-unique index)
schema.config	16	
schema.config	17	
schema.config	18	--                              DATA TYPES
schema.config	19	--                              ----------
schema.config	20	--                              int             : 4-bytes signed integer
schema.config	21	--                              long_int        : 8-bytes signed integer
schema.config	22	--                              decimal(p,s)    : decimal number, p for precision, s for scale
schema.config	23	--                              char            : one character
schema.config	24	--                              str(n)          : string of length n, n is limited to 2000
schema.config	25	--                              long_str(n)     : long text of length n (CLOB)
schema.config	26	--                              uni_str(n)      : unicode string of length n, n is limited to 1000
schema.config	27	--                              long_uni_str(n) : long unicode text of length n (NCLOB)
schema.config	28	--                              timestamp       : date and time with 1 second precision, without time zone
schema.config	29	--                              boolean         : boolean
schema.config	30	
schema.config	31	
schema.config	32	--                              FIELD OPTIONS
schema.config	33	--                              -------------
schema.config	34	--                              default <value> : specifies default value
schema.config	35	--                              defines         : defines value of it's domain
schema.config	36	--                              refers          : references to domain values defines in other tables
schema.config	37	--                              serial          : means that this field gets it values from a sequence
schema.config	38	
schema.config	39	
schema.config	40	--                              DOMAIN OPTIONS
schema.config	41	--                              -------------
schema.config	42	--                              persistable     : these domains store sequence id in domain_sequence table
schema.config	43	--                                                It's possible to specify the number of ids reserved by a server at once
schema.config	44	--                                                using CachingNumericIdSequences.DEFAULT_IDS_COUNTS_TO_RESERVE_PER_DOMAIN
schema.config	45	--                                                Default value is 0 meaning that nodes will access the database every time they need a new id
schema.config	46	
schema.config	47	
schema.config	48	-- Notes:
schema.config	228	
schema.config	49	-- 1. The first table should be the 'db_version', the last table should be the 'server'.
schema.config	50	-- 2. Tables are grouped by categories. Please put new one into a proper category.
schema.config	51	-- 3. All names should be written in lower case; national characters are not allowed.
schema.config	52	-- 4. Names of temporary tables are suffixed with '$'.
schema.config	53	-- 5. Ensure that there are no tab characters at beginning of lines.
schema.config	54	-- 6. Unicode string and text types are used since 9.0.
schema.config	55	-- 7. BLOBs (strings more than 4000 or unicode strings more than 2000) must be the last fields.
schema.config	56	-- 8. In this file allows at most 160 character per line.
schema.config	57	
schema.config	58	
schema.config	59	
schema.config	60	-- DOMAINS
schema.config	61	
schema.config	62	config_id:                 domain of str(80)
schema.config	63	group_id:                  domain of str(80)
schema.config	64	teamcity_username:         domain of str(60)
schema.config	65	user_id:                   domain of long_int persistable
schema.config	66	user_role_id:              domain of str(80)
schema.config	67	user_notific_rule_id:      domain of long_int persistable
schema.config	68	group_notific_rule_id:     domain of long_int persistable
schema.config	69	subject_property_key:      domain of str(80)
schema.config	70	agent_pool_id:             domain of int persistable
schema.config	71	agent_id:                  domain of int persistable
schema.config	72	agent_type_id:             domain of int persistable
schema.config	73	project_ext_id:            domain of str(240)
schema.config	74	bt_ext_id:                 domain of str(240)
schema.config	75	vcs_root_ext_id:           domain of str(240)
schema.config	76	project_int_id:            domain of str(80)
schema.config	77	bt_int_id:                 domain of str(80)  -- for both build_type and build_template
schema.config	78	ids_group_id:              domain of int
schema.config	79	vcs_root_int_id:           domain of int persistable
schema.config	80	vcs_root_instance_id:      domain of int
schema.config	81	vcs_revision:              domain of str(200)
schema.config	82	modification_hash:         domain of str(40)
schema.config	83	modification_id:           domain of long_int persistable
schema.config	84	personal_modification_id:  domain of long_int persistable
schema.config	85	promotion_id:              domain of long_int persistable
schema.config	86	build_id:                  domain of long_int persistable
schema.config	87	test_name_hash:            domain of long_int
schema.config	88	test_metadata_type_key:    domain of int
schema.config	89	test_metadata_key:         domain of long_int persistable
schema.config	90	problem_id:                domain of int
schema.config	91	problem_type:              domain of str(80)
schema.config	92	problem_identity:          domain of str(60)
schema.config	93	mute_id:                   domain of int persistable
schema.config	94	file_name_hash:            domain of str(40)
schema.config	95	tag_phrase:                domain of uni_str(255)
schema.config	96	metric_hash:               domain of long_int
schema.config	97	server_health_item_id:     domain of long_int persistable
schema.config	98	audit_comment_id:          domain of long_int persistable
schema.config	99	cleanup_proc_id:           domain of long_int persistable
schema.config	100	inspection_id_hash:        domain of long_int
schema.config	101	inspection_id_str:         domain of str(255)
schema.config	102	inspection_data_hash:      domain of long_int
schema.config	103	duplicate_file_id:         domain of long_int
schema.config	104	duplicate_file_name:       domain of str(255)
schema.config	105	duplicate_result_id:       domain of long_int
schema.config	106	duplicate_result_hash:     domain of int
schema.config	107	dependency_id:             domain of str(40)
schema.config	108	custom_data_body_id:       domain of long_int
schema.config	109	permanent_token_id:        domain of long_int persistable
schema.config	110	node_task_id:              domain of int persistable
schema.config	111	
schema.config	112	
schema.config	113	
schema.config	114	
schema.config	115	-- TABLES THAT CONTAIN PREDEFINED IMMUTABLE DATA
schema.config	116	
schema.config	117	db_version:                     table
schema.config	118	
schema.config	119	  version_number:               int M
schema.config	120	  version_time:                 timestamp M
schema.config	121	  incompatible_change:          boolean M default 1
schema.config	122	
schema.config	123	  db_version_pk:                PK (version_number)
schema.config	124	
schema.config	125	
schema.config	126	meta_file_line:                 table
schema.config	127	
schema.config	128	  file_name:                    str(15)  M  -- metadata file name with suffix
schema.config	129	  line_nr:                      int      M  -- line number, started with 1
schema.config	130	  line_text:                    str(160)    -- text of line
schema.config	131	
schema.config	132	  meta_file_line_pk:            PK (file_name, line_nr)
schema.config	133	
schema.config	134	
schema.config	135	single_row:                     table
schema.config	136	
schema.config	137	  dummy_field:                  char
schema.config	138	
schema.config	139	
schema.config	140	server_property:                table
schema.config	141	
schema.config	142	  prop_name:                    str(80) M
schema.config	143	  prop_value:                   str(256) M
schema.config	144	
schema.config	145	  server_property_pk:           PK (prop_name)
schema.config	146	
schema.config	147	
schema.config	148	
schema.config	149	
schema.config	150	-- GLOBAL TABLES (NOT RELATED TO PROJECTS OR USERS OR AGENTS)
schema.config	151	
schema.config	152	
schema.config	153	db_heartbeat:                   table
schema.config	154	
schema.config	155	  node_id:                      str(80)       -- non mandatory for backward compatibility with previous versions
schema.config	156	  starting_code:                long_int M
schema.config	157	  starting_time:                timestamp     -- not used starting with 2020.2
schema.config	158	  lock_mode:                    char M
schema.config	159	  ip_address:                   str(80)
schema.config	160	  additional_info:              str(2000)
schema.config	161	  last_time:                    timestamp     -- not used starting with 2020.2
schema.config	162	  update_interval:              long_int      -- non mandatory for backward compatibility with previous versions
schema.config	163	  uuid:                         str(80)       -- non mandatory for backward compatibility with previous versions
schema.config	164	  app_type:                     str(80)       -- non mandatory for backward compatibility with previous versions
schema.config	165	  url:                          str(128)      -- non mandatory for backward compatibility with previous versions
schema.config	166	  access_token:                 str(80)       -- not used starting with 2020.1
schema.config	167	  build_number:                 str(80)       -- non mandatory for backward compatibility with previous versions
schema.config	168	  display_version:              str(80)       -- non mandatory for backward compatibility with previous versions
schema.config	169	  responsibilities:             long_int      -- non mandatory for backward compatibility with previous versions
schema.config	170	  unix_last_time:               long_int      -- non mandatory for backward compatibility with previous versions
schema.config	171	  unix_starting_time:           long_int      -- non mandatory for backward compatibility with previous versions
schema.config	172	
schema.config	173	  db_heartbeat_pk:              PK (starting_code)
schema.config	174	
schema.config	175	
schema.config	176	backup_info:                    table
schema.config	177	
schema.config	178	  mproc_id:                     int M
schema.config	179	  file_name:                    str(1000)
schema.config	180	  file_size:                    long_int
schema.config	181	  started:                      timestamp M
schema.config	182	  finished:                     timestamp
schema.config	183	  status:                       char
schema.config	184	
schema.config	185	  backup_info_pk:               PK (mproc_id)
schema.config	186	
schema.config	187	  backup_info_file_i:           IE (file_name)
schema.config	188	
schema.config	189	
schema.config	190	backup_builds:                  table
schema.config	191	
schema.config	192	  build_id:                     build_id M
schema.config	193	
schema.config	194	  backup_builds_pk:             PK (build_id)
schema.config	195	
schema.config	196	
schema.config	197	cleanup_history:                table
schema.config	198	
schema.config	199	  proc_id:                      cleanup_proc_id M serial defines
schema.config	200	  start_time:                   long_int M
schema.config	201	  finish_time:                  long_int
schema.config	202	  interrupt_reason:             str(20)
schema.config	203	
schema.config	204	  cleanup_history_pk:           PK (proc_id)
schema.config	205	
schema.config	206	
schema.config	207	domain_sequence:               table
schema.config	208	
schema.config	209	  domain_name:                  str(100) M
schema.config	210	  last_used_value:              long_int M
schema.config	211	
schema.config	212	  domain_name_pk:               PK (domain_name)
schema.config	213	
schema.config	214	-- AGENT RELATED TABLES
schema.config	215	
schema.config	216	-- PROJECTS AND BUILD HISTORY TABLES
schema.config	217	
schema.config	218	
schema.config	219	project:                        dictionary table
schema.config	220	
schema.config	221	  int_id:                       project_int_id   M  serial   defines
schema.config	222	  config_id:                    config_id        M           defines
schema.config	223	  origin_project_id:            project_int_id
schema.config	224	  delete_time:                  long_int
schema.config	225	
schema.config	226	  project_pk:                   PK (int_id)
schema.config	227	  project_ak:                   AK (config_id)               stable
schema.config	229	
schema.config	230	build_type:                     dictionary table
schema.config	231	
schema.config	232	  int_id:                       bt_int_id        M  serial   defines
schema.config	233	  config_id:                    config_id        M           defines
schema.config	234	  origin_project_id:            project_int_id
schema.config	235	  delete_time:                  long_int
schema.config	236	
schema.config	237	  build_type_pk:                PK (int_id)
schema.config	238	  build_type_ak:                AK (config_id)               stable
schema.config	239	
schema.config	240	
schema.config	241	vcs_root:                       dictionary table
schema.config	242	
schema.config	243	  int_id:                       vcs_root_int_id  M  serial   defines
schema.config	244	  config_id:                    config_id        M           defines
schema.config	245	  origin_project_id:            project_int_id
schema.config	246	  delete_time:                  long_int
schema.config	247	
schema.config	248	  vcs_root_pk:                  PK (int_id)
schema.config	249	  vcs_root_ak:                  AK (config_id)               stable
schema.config	250	
schema.config	251	
schema.config	252	project_mapping:                table
schema.config	253	
schema.config	254	  int_id:                       project_int_id M       refers
schema.config	255	  ext_id:                       project_ext_id M       defines
schema.config	256	  main:                         boolean M
schema.config	257	
schema.config	258	  project_mapping_pk:           PK (int_id, ext_id)
schema.config	259	  project_mapping_ak:           AK (ext_id)
schema.config	260	
schema.config	261	
schema.config	262	build_type_mapping:             table
schema.config	263	
schema.config	264	  int_id:                       bt_int_id M            refers
schema.config	265	  ext_id:                       bt_ext_id M            defines
schema.config	266	  main:                         boolean M
schema.config	267	
schema.config	268	  build_type_mapping_pk:        PK (int_id, ext_id)
schema.config	269	  build_type_mapping_ak:        AK (ext_id)
schema.config	270	
schema.config	271	
schema.config	272	vcs_root_mapping:               table
schema.config	273	
schema.config	274	  int_id:                       vcs_root_int_id M      defines -- refers    TODO return refers when vcs_root table is populated
schema.config	275	  ext_id:                       vcs_root_ext_id M      defines
schema.config	276	  main:                         boolean M
schema.config	277	
schema.config	278	  vcs_root_mapping_pk:          PK (int_id, ext_id)
schema.config	279	  vcs_root_mapping_ak:          AK (ext_id)
schema.config	280	
schema.config	281	
schema.config	282	build_type_counters:            table
schema.config	283	   build_type_id:               bt_int_id M
schema.config	284	   counter:                     long_int M
schema.config	285	
schema.config	286	   build_type_counters_pk:      PK(build_type_id)
schema.config	287	
schema.config	288	agent_pool:                     table
schema.config	289	
schema.config	290	  agent_pool_id:                agent_pool_id M  serial defines   -- 0 means the Default Pool
schema.config	291	  agent_pool_name:              uni_str(191)            defines   -- see jetbrains.buildServer.serverSide.agentPools.AgentPoolConstants.MAX_POOL_NAME_LENGTH
schema.config	292	  min_agents:                   int
schema.config	293	  max_agents:                   int                       -- -1 means unlimited
schema.config	294	  owner_project_id:             project_int_id          refers
schema.config	295	
schema.config	296	  agent_pool_id_pk:             PK (agent_pool_id)
schema.config	297	
schema.config	298	
schema.config	299	agent_type:                     table
schema.config	300	
schema.config	301	  agent_type_id:                agent_type_id M  serial defines
schema.config	302	  agent_pool_id:                agent_pool_id M         refers
schema.config	303	  cloud_code:                   str(6) M
schema.config	304	  profile_id:                   str(30) M
schema.config	305	  image_id:                     str(60) M
schema.config	306	  policy:                       int M       -- 1: all configurations, 2: selected ones only
schema.config	307	
schema.config	308	  agent_type_pk:                PK (agent_type_id)
schema.config	309	  agent_type_ak:                AK (cloud_code, profile_id, image_id)
schema.config	310	  agent_type_pool_i:            IE (agent_pool_id)
schema.config	311	
schema.config	312	
schema.config	313	agent_type_info:                table
schema.config	314	
schema.config	315	  agent_type_id:                agent_type_id M  refers
schema.config	316	  os_name:                      str(60) M
schema.config	317	  cpu_rank:                     int
schema.config	318	  created_timestamp:            timestamp
schema.config	319	  modified_timestamp:           timestamp
schema.config	320	
schema.config	321	  agent_type_info_pk:           PK (agent_type_id)
schema.config	322	
schema.config	323	
schema.config	324	agent_type_runner:              table
schema.config	325	
schema.config	326	  agent_type_id:                agent_type_id M  refers
schema.config	327	  runner:                       str(250) M
schema.config	328	
schema.config	329	  agent_type_runner_pk:         PK (agent_type_id, runner)
schema.config	330	
schema.config	331	
schema.config	332	agent_type_vcs:                 table
schema.config	333	
schema.config	334	  agent_type_id:                agent_type_id M  refers
schema.config	335	  vcs:                          str(250) M
schema.config	336	
schema.config	337	  agent_type_vcs_pk:            PK (agent_type_id, vcs)
schema.config	338	
schema.config	339	
schema.config	340	agent_type_param:               table
schema.config	341	
schema.config	342	  agent_type_id:                agent_type_id M  refers
schema.config	343	  param_kind:                   char M
schema.config	344	  param_name:                   str(160) M
schema.config	345	  param_value:                  uni_str(2000)
schema.config	346	
schema.config	347	  agent_type_param_pk:          PK (agent_type_id, param_kind, param_name)
schema.config	348	
schema.config	349	
schema.config	350	agent:                          table
schema.config	351	
schema.config	352	  id:                           agent_id M       serial defines
schema.config	353	  name:                         uni_str(256) M               defines
schema.config	354	  host_addr:                    str(80) M
schema.config	355	  port:                         int M
schema.config	356	  agent_type_id:                int M
schema.config	357	  status:                       int
schema.config	358	  authorized:                   int
schema.config	359	  registered:                   int
schema.config	360	  registration_timestamp:       long_int
schema.config	361	  last_binding_timestamp:       long_int
schema.config	362	  unregistered_reason:          str(256)
schema.config	363	  authorization_token:          str(32)
schema.config	364	  status_to_restore:            int
schema.config	365	  status_restoring_timestamp:   long_int
schema.config	366	  version:                      str(80)
schema.config	367	  plugins_version:              str(80)
schema.config	368	
schema.config	369	  agent_pk:                     PK (id)
schema.config	370	  agent_name_ui:                AK (name)
schema.config	371	
schema.config	372	  agent_host_address:           IE (host_addr)
schema.config	373	  agent_authorization_token:    IE (authorization_token)
schema.config	374	  agent_agent_type_id:          IE (agent_type_id)
schema.config	375	
schema.config	376	cloud_image_state:              table
schema.config	377	  project_id:                   project_int_id M        refers
schema.config	378	  profile_id:                   str(30) M
schema.config	379	  image_id:                     str(80) M
schema.config	380	  name:                         str(80) M
schema.config	381	
schema.config	382	  cloud_image_state_pk:         PK (project_id, profile_id, image_id)
schema.config	383	
schema.config	384	cloud_instance_state:           table
schema.config	385	  project_id:                   project_int_id M        refers
schema.config	386	  profile_id:                   str(30) M
schema.config	387	  image_id:                     str(80) M
schema.config	388	  instance_id:                  str(80) M
schema.config	389	  name:                         str(80) M
schema.config	390	  last_update:                  timestamp M
schema.config	391	  status:                       str(30) M
schema.config	392	  start_time:                   timestamp M
schema.config	393	  network_identity:             str(80)
schema.config	394	  is_expired:                   boolean
schema.config	395	  agent_id:                     agent_id
schema.config	396	
schema.config	397	  cloud_instance_pk:            PK (project_id, profile_id, image_id, instance_id)
schema.config	398	
schema.config	399	cloud_state_data:               table
schema.config	400	  project_id:                   project_int_id M        refers
schema.config	401	  profile_id:                   str(30) M
schema.config	402	  image_id:                     str(80) M
schema.config	403	  instance_id:                  str(80) M
schema.config	404	  data:                         uni_str(2000) M
schema.config	405	
schema.config	406	  cloud_state_data_pk:          PK (project_id, profile_id, image_id, instance_id)
schema.config	407	
schema.config	408	--deprecated table
schema.config	409	cloud_started_instance:         table
schema.config	410	
schema.config	411	  profile_id:                   str(30) M
schema.config	412	  cloud_code:                   str(6) M
schema.config	413	  image_id:                     str(80) M
schema.config	414	  instance_id:                  str(80) M
schema.config	415	  last_update:                  timestamp M
schema.config	416	
schema.config	417	  cloud_started_instance_pk:    PK (profile_id, cloud_code, image_id, instance_id)
schema.config	418	
schema.config	419	
schema.config	420	cloud_image_without_agent:      table
schema.config	421	
schema.config	422	  profile_id:                   str(30) M
schema.config	423	  cloud_code:                   str(6) M
schema.config	424	  image_id:                     str(80) M
schema.config	425	  last_update:                  timestamp M
schema.config	426	
schema.config	427	  cloud_image_without_agent_pk: PK (profile_id, cloud_code, image_id)
schema.config	428	
schema.config	429	
schema.config	430	
schema.config	431	
schema.config	432	-- USERS DEFINITION TABLES
schema.config	433	
schema.config	434	
schema.config	435	usergroups:                     dictionary table
schema.config	436	
schema.config	437	  group_id:                     group_id      M      defines
schema.config	438	  name:                         uni_str(255)  M      defines
schema.config	439	  description:                  uni_str(2000)
schema.config	440	
schema.config	441	  usergroups_pk:                PK (group_id)        stable
schema.config	442	  usergroups_ak:                AK (name)
schema.config	443	
schema.config	444	
schema.config	445	usergroup_property:             table
schema.config	446	
schema.config	447	  group_id:                     group_id M               refers
schema.config	448	  prop_key:                     subject_property_key  M  defines
schema.config	449	  prop_value:                   uni_str(2000)
schema.config	450	
schema.config	451	  usergroup_property_pk:        PK (group_id, prop_key)
schema.config	452	
schema.config	453	
schema.config	454	users:                          dictionary table
schema.config	455	
schema.config	456	  id:                           user_id    M  serial defines  -- user's surrogate id
schema.config	457	  username:                     teamcity_username  M defines  -- user's natural id, in lower case
schema.config	458	  password:                     str(128)                      -- enciphered password
schema.config	459	  name:                         uni_str(256)                  -- user's real name
schema.config	460	  email:                        str(256)
schema.config	461	  last_login_timestamp:         long_int
schema.config	462	  algorithm:                    str(20)
schema.config	463	
schema.config	464	  users_pk:                     PK (id)
schema.config	465	  users_ak:                     AK (username) stable
schema.config	466	
schema.config	467	
schema.config	468	user_property:                  table
schema.config	469	
schema.config	470	  user_id:                      user_id                M  refers
schema.config	471	  prop_key:                     subject_property_key   M  defines
schema.config	472	  prop_value:                   uni_str(2000)
schema.config	473	  locase_value_hash:            long_int               -- store standart javas hashcode of value in lower case (i.e. prop_value.toLowerCase().hashCode())
schema.config	474	
schema.config	475	  user_property_pk:             PK (user_id, prop_key)
schema.config	476	  user_property_key_value_idx:  IE (prop_key, locase_value_hash)
schema.config	477	
schema.config	478	
schema.config	479	user_attribute:                 table
schema.config	480	
schema.config	481	  user_id:                      user_id                M  refers
schema.config	482	  attr_key:                     str(80)                M
schema.config	483	  attr_value:                   uni_str(2000)
schema.config	484	
schema.config	485	  user_attr_pk:                 PK (user_id, attr_key)
schema.config	486	
schema.config	487	
schema.config	488	user_blocks:                    table
schema.config	489	
schema.config	490	  user_id:                      user_id M  refers
schema.config	491	  block_type:                   str(80) M
schema.config	492	  state:                        str(2048)
schema.config	493	
schema.config	494	  user_blocks_pk:               PK (user_id, block_type)
schema.config	495	
schema.config	496	
schema.config	497	user_notification_events:       table
schema.config	498	
schema.config	499	  id:                           user_notific_rule_id M  serial defines
schema.config	500	  user_id:                      user_id M                      refers
schema.config	501	  notificator_type:             str(20) M
schema.config	502	  events_mask:                  int M
schema.config	503	
schema.config	504	  user_notification_events_pk:  PK (id)
schema.config	505	
schema.config	506	  notification_events_notifier: IE (notificator_type)
schema.config	507	  notification_events_user_id:  IE (user_id)
schema.config	508	
schema.config	509	
schema.config	510	user_watch_type:                table
schema.config	511	
schema.config	512	  rule_id:                      user_notific_rule_id M   refers
schema.config	513	  user_id:                      user_id M                refers
schema.config	514	  notificator_type:             str(20) M
schema.config	515	  watch_type:                   int M    -- values 1..5; 2 - project, 3 - build type, other - unknown
schema.config	516	  watch_value:                  str(80) M
schema.config	517	  order_num:                    long_int
schema.config	518	
schema.config	519	  user_watch_type_pk:           IE (user_id, notificator_type, watch_type, watch_value)
schema.config	520	  watch_type_rule_id:           IE (rule_id)
schema.config	521	
schema.config	522	
schema.config	523	user_notification_data:         table
schema.config	524	  user_id:                      user_id M                refers
schema.config	525	  rule_id:                      user_notific_rule_id M   refers
schema.config	526	  additional_data:              str(2000)
schema.config	527	
schema.config	528	  user_notif_data_pk:           PK (user_id, rule_id)
schema.config	529	  user_notif_data_rule_id:      IE (rule_id)
schema.config	530	
schema.config	531	
schema.config	532	usergroup_subgroups:            table
schema.config	533	
schema.config	534	  hostgroup_id:                 group_id M
schema.config	535	  subgroup_id:                  group_id M
schema.config	536	
schema.config	537	  usergroup_subgroups_pk:       PK (hostgroup_id, subgroup_id)
schema.config	538	
schema.config	539	
schema.config	540	usergroup_users:                table
schema.config	541	
schema.config	542	  group_id:                     group_id M  refers
schema.config	543	  user_id:                      user_id M   refers
schema.config	544	
schema.config	545	  usergroup_users_pk:           PK (group_id, user_id)
schema.config	546	
schema.config	547	
schema.config	548	usergroup_notification_events:  table
schema.config	549	
schema.config	550	  id:                           group_notific_rule_id M  serial defines
schema.config	551	  group_id:                     group_id M                      refers
schema.config	552	  notificator_type:             str(20) M
schema.config	553	  events_mask:                  int M
schema.config	554	
schema.config	555	  usergroup_notific_evnts_pk:   PK (id)
schema.config	556	
schema.config	557	  usergroup_events_notifier:    IE (notificator_type)
schema.config	558	  usergroup_events_group_id:    IE (group_id)
schema.config	559	
schema.config	560	
schema.config	561	usergroup_watch_type:           table
schema.config	562	
schema.config	563	  rule_id:                      group_notific_rule_id M refers
schema.config	564	  group_id:                     group_id M              refers
schema.config	565	  notificator_type:             str(20) M
schema.config	566	  watch_type:                   int M    -- values 1..5; 2 - project, 3 - build type, other - unknown
schema.config	567	  watch_value:                  str(80) M
schema.config	568	  order_num:                    long_int
schema.config	569	
schema.config	570	  usergroup_watch_type_pk:      IE (group_id, notificator_type, watch_type, watch_value)
schema.config	571	  group_watch_type_rule_id:     IE (rule_id)
schema.config	572	
schema.config	573	
schema.config	574	usergroup_notification_data:    table
schema.config	575	
schema.config	576	  group_id:                     group_id M               refers
schema.config	577	  rule_id:                      group_notific_rule_id M  refers
schema.config	578	  additional_data:              str(2000)
schema.config	579	
schema.config	580	  group_notif_data_pk:          PK (group_id, rule_id)
schema.config	581	  group_notif_data_rule_id:     IE (rule_id)
schema.config	582	
schema.config	583	
schema.config	584	remember_me:                    table
schema.config	585	
schema.config	586	  user_key:                     str(65) M
schema.config	587	  secure:                       long_int M
schema.config	588	
schema.config	589	  remember_me_uk_secure_idx:    IE (user_key, secure)
schema.config	590	  remember_me_secure_idx:       IE (secure)
schema.config	591	
schema.config	592	
schema.config	593	permanent_tokens:               table
schema.config	594	
schema.config	595	  id:                           permanent_token_id M serial defines
schema.config	596	  identifier:                   str(36) M
schema.config	597	  name:                         uni_str(128) M
schema.config	598	  user_id:                      user_id M refers
schema.config	599	  hashed_value:                 str(255) M
schema.config	600	  expiration_time:              long_int
schema.config	601	  creation_time:                long_int
schema.config	602	  last_access_time:             long_int
schema.config	603	  last_access_info:             str(255)
schema.config	604	
schema.config	605	  permanent_token_pk:           PK (id)
schema.config	606	  token_user_id_name_ak:        AK (user_id, name)
schema.config	607	  token_identifier_ak:          AK (identifier)
schema.config	608	  permanent_t_exp_t_idx:        IE (expiration_time)
schema.config	609	
schema.config	610	
schema.config	967	
schema.config	611	permanent_token_permissions:    table
schema.config	612	
schema.config	613	  id:                           permanent_token_id M refers
schema.config	614	  project_id:                   project_int_id
schema.config	615	  permission:                   int M
schema.config	616	
schema.config	617	  token_permissions_pk:         PK(id, project_id, permission)
schema.config	618	  permanent_t_p_pr_id_idx:      IE (project_id)
schema.config	619	
schema.config	620	
schema.config	621	-- COMMON DICTIONARY TABLES
schema.config	622	
schema.config	623	
schema.config	624	long_file_name:                 dictionary table
schema.config	625	
schema.config	626	  hash:                         file_name_hash M       defines
schema.config	627	  file_name:                    long_uni_str(16000) M  defines
schema.config	628	
schema.config	629	  long_file_name_pk:            PK (hash) stable
schema.config	630	
schema.config	631	
schema.config	632	test_names:                     dictionary table
schema.config	633	
schema.config	634	  id:                           test_name_hash M       defines
schema.config	635	  test_name:                    uni_str(1024) M         defines
schema.config	636	  order_num:                    long_int
schema.config	637	
schema.config	638	  test_names_pk:                PK (id) stable
schema.config	639	  order_num_idx:                IE (order_num)
schema.config	640	
schema.config	641	
schema.config	642	data_storage_dict:              dictionary table
schema.config	643	
schema.config	644	  metric_id:                    metric_hash M          defines
schema.config	645	  value_type_key:               str(200)               defines
schema.config	646	
schema.config	647	  metric_id_pk:                 PK (metric_id)         stable
schema.config	648	  value_type_key_index:         AK (value_type_key)    stable
schema.config	649	
schema.config	650	
schema.config	651	problem:                        dictionary table
schema.config	652	
schema.config	653	  problem_id:                   problem_id       M serial defines
schema.config	654	  problem_type:                 problem_type     M
schema.config	655	  problem_identity:             problem_identity M
schema.config	656	
schema.config	657	  problem_pk:                   PK (problem_id)
schema.config	658	  problem_ak:                   AK (problem_type, problem_identity) stable
schema.config	659	
schema.config	660	
schema.config	661	agent_type_bt_access:           table
schema.config	662	
schema.config	663	  agent_type_id:                agent_type_id M        refers
schema.config	664	  build_type_id:                bt_int_id M            refers
schema.config	665	
schema.config	666	  agent_type_bt_access_pk:      PK (agent_type_id, build_type_id)
schema.config	667	  agent_type_bt_access_bt_i:    IE (build_type_id)
schema.config	668	
schema.config	669	
schema.config	670	user_projects_visibility:       table
schema.config	671	
schema.config	672	  user_id:                      user_id M              refers
schema.config	673	  project_int_id:               project_int_id M       refers
schema.config	674	  visible:                      int M
schema.config	675	
schema.config	676	  user_projects_visibility_pk:  PK (user_id, project_int_id)
schema.config	677	  user_projects_visibility_i:   IE (project_int_id)
schema.config	678	
schema.config	679	
schema.config	680	user_projects_order:            table
schema.config	681	
schema.config	682	  user_id:                      user_id M              refers
schema.config	683	  project_int_id:               project_int_id M       refers
schema.config	684	  ordernum:                     int
schema.config	685	
schema.config	686	  user_projects_order_pk:       PK (user_id, project_int_id)
schema.config	687	  user_projects_order_i:        IE (project_int_id)
schema.config	688	
schema.config	689	user_build_types_order:         table
schema.config	690	
schema.config	691	  user_id:                      user_id M              refers
schema.config	692	  project_int_id:               project_int_id M       refers
schema.config	693	  bt_int_id:                    bt_int_id M            refers
schema.config	694	  ordernum:                     int M
schema.config	695	  visible:                      int M
schema.config	696	
schema.config	697	  user_bt_order_pk:             PK (user_id, project_int_id, bt_int_id)
schema.config	698	  user_build_types_order_i:     IE (project_int_id)
schema.config	699	
schema.config	700	vcs_root_instance:              table
schema.config	701	
schema.config	702	  id:                           vcs_root_instance_id M  serial defines
schema.config	703	  parent_id:                    vcs_root_int_id M              refers
schema.config	704	  settings_hash:                long_int M
schema.config	705	  body:                         long_str(16384)
schema.config	706	
schema.config	1061	
schema.config	707	  vcs_root_instance_pk:         PK (id)
schema.config	708	  vcs_root_instance_parent_idx: IE (parent_id, settings_hash)
schema.config	709	
schema.config	710	
schema.config	711	agent_pool_project:             table
schema.config	712	
schema.config	713	  agent_pool_id:                agent_pool_id M         refers
schema.config	714	  project_int_id:               project_int_id M        refers
schema.config	715	
schema.config	716	  agent_pool_project_pk:        PK (agent_pool_id, project_int_id)
schema.config	717	
schema.config	718	
schema.config	719	vcs_history:                    table
schema.config	720	
schema.config	721	  modification_id:              modification_id M       serial defines
schema.config	722	  user_name:                    uni_str(255)
schema.config	723	  description:                  uni_str(2000)
schema.config	724	  change_date:                  long_int
schema.config	725	  register_date:                long_int
schema.config	726	  vcs_root_id:                  vcs_root_instance_id           refers
schema.config	727	  changes_count:                int
schema.config	728	  version:                      vcs_revision M
schema.config	729	  display_version:              str(200)
schema.config	730	
schema.config	731	  vcs_history_pk:               PK (modification_id)
schema.config	732	
schema.config	733	  vcs_history_root_id_mod_id_i: IE (vcs_root_id, modification_id)
schema.config	734	  vcs_history_date_i:           IE (register_date)
schema.config	735	
schema.config	736	
schema.config	737	vcs_change:                     table
schema.config	738	
schema.config	739	  modification_id:              modification_id M   refers
schema.config	740	  file_num:                     int M                           -- number of file in the change
schema.config	741	  vcs_file_name:                uni_str(2000) M                 -- first 2000 characters if the name is long
schema.config	742	  vcs_file_name_hash:           file_name_hash      refers
schema.config	743	  relative_file_name_pos:       int
schema.config	744	  relative_file_name:           uni_str(2000)
schema.config	745	  relative_file_name_hash:      file_name_hash      refers
schema.config	746	  change_type:                  int M
schema.config	747	  change_name:                  str(64)
schema.config	748	  before_revision:              vcs_revision
schema.config	749	  after_revision:               vcs_revision
schema.config	750	
schema.config	751	  vcs_change_pk:                PK (modification_id, file_num)
schema.config	752	
schema.config	753	
schema.config	754	personal_vcs_history:           table
schema.config	755	
schema.config	756	  modification_id:              personal_modification_id M serial defines
schema.config	757	  modification_hash:            modification_hash M               defines
schema.config	758	  user_id:                      user_id           M               refers
schema.config	759	  description:                  uni_str(2000)
schema.config	760	  change_date:                  long_int          M
schema.config	761	  changes_count:                int               M
schema.config	762	  commit_changes:               int
schema.config	763	  status:                       int               M default 0
schema.config	764	  scheduled_for_deletion:       boolean           M default 0
schema.config	765	
schema.config	766	  personal_vcs_history_pk:      PK (modification_id)
schema.config	767	  personal_vcs_history_ak:      AK (modification_hash)      stable
schema.config	768	  personal_vcs_history_user_i:  IE (user_id)
schema.config	769	
schema.config	770	
schema.config	771	personal_vcs_change:            table
schema.config	772	
schema.config	773	  modification_id:              personal_modification_id M  refers
schema.config	774	  file_num:                     int M                               -- number of file in the change
schema.config	775	  vcs_file_name:                uni_str(2000) M                     -- first 2000 characters if the name is long
schema.config	776	  vcs_file_name_hash:           file_name_hash              refers
schema.config	777	  relative_file_name_pos:       int
schema.config	778	  relative_file_name:           uni_str(2000)
schema.config	779	  relative_file_name_hash:      file_name_hash              refers
schema.config	780	  change_type:                  int M
schema.config	781	  change_name:                  str(64)
schema.config	782	  before_revision:              vcs_revision
schema.config	783	  after_revision:               vcs_revision
schema.config	784	
schema.config	785	  personal_vcs_changes_pk:      PK (modification_id, file_num)
schema.config	786	
schema.config	787	
schema.config	788	vcs_changes_graph:              table
schema.config	789	
schema.config	790	  child_modification_id:        modification_id M   refers
schema.config	791	  child_revision:               vcs_revision M
schema.config	792	  parent_num:                   int M
schema.config	793	  parent_modification_id:       modification_id     refers
schema.config	794	  parent_revision:              vcs_revision M
schema.config	795	
schema.config	796	  vcs_changes_graph_pk:         PK (child_modification_id, parent_num)
schema.config	797	  vcs_changes_graph_parent_i:   IE (parent_modification_id)
schema.config	798	
schema.config	799	
schema.config	800	vcs_change_attrs:               table
schema.config	801	
schema.config	802	  modification_id:              modification_id M   refers
schema.config	803	  attr_name:                    str(200) M
schema.config	804	  attr_value:                   str(1000)
schema.config	805	
schema.config	806	  vcs_change_attrs_pk:          PK (modification_id, attr_name)
schema.config	807	
schema.config	808	
schema.config	809	vcs_root_first_revision:        table
schema.config	810	
schema.config	811	  build_type_id:                bt_int_id M        refers
schema.config	812	  parent_root_id:               vcs_root_int_id M  refers
schema.config	813	  settings_hash:                long_int M
schema.config	814	  vcs_revision:                 vcs_revision M
schema.config	815	
schema.config	816	  vcs_root_first_revision_pk:   PK (build_type_id, parent_root_id, settings_hash)
schema.config	817	
schema.config	818	
schema.config	819	vcs_username:                   table
schema.config	820	
schema.config	821	  user_id:                      user_id M refers
schema.config	822	  vcs_name:                     str(60) M
schema.config	823	  parent_vcs_root_id:           vcs_root_int_id M refers
schema.config	824	  order_num:                    int M
schema.config	825	  username:                     uni_str(255) M
schema.config	826	
schema.config	827	  vcs_username_pk:              PK (user_id, vcs_name, parent_vcs_root_id, order_num)
schema.config	828	  vcs_username_ak:              AK (user_id, vcs_name, parent_vcs_root_id, username)
schema.config	829	  vcs_username_user_ie:         IE (vcs_name, parent_vcs_root_id, username)
schema.config	830	
schema.config	831	
schema.config	832	build_state:                    table
schema.config	833	
schema.config	834	  id:                           promotion_id M   serial defines
schema.config	835	  build_id:                     build_id         defines
schema.config	836	  build_type_id:                bt_int_id        refers
schema.config	837	  modification_id:              modification_id  refers    -- can be null if changes checking was not performed yet,
schema.config	838	                                                           -- equals -1 if changes collecting is performed but there were no changes
schema.config	839	                                                           -- detected and there were no changes in the build configuration since its creation
schema.config	840	  chain_modification_id:        modification_id  refers    -- see docs for BuildPromotionEx.getChainModificationId()
schema.config	841	  personal_modification_id:     personal_modification_id   -- reference to personal_vcs_history, or null for regular builds
schema.config	842	  personal_user_id:             user_id                    -- owner of the modification
schema.config	843	  is_personal:                  boolean M default 0
schema.config	844	  is_canceled:                  boolean M default 0
schema.config	845	  is_changes_detached:          boolean M default 0
schema.config	846	  is_deleted:                   boolean M default 0
schema.config	847	  branch_name:                  uni_str(1024)                   -- null means no branch (master)
schema.config	848	  queued_time:                  long_int
schema.config	849	  remove_from_queue_time:       long_int
schema.config	850	
schema.config	851	  build_state_pk:               PK (id)
schema.config	852	
schema.config	853	  build_state_build_i:          IE (build_id, is_deleted, branch_name, is_personal)
schema.config	854	  build_state_build_type_i:     IE (build_type_id, is_deleted, branch_name, is_personal)
schema.config	855	  build_state_mod_i:            IE (modification_id)       -- used when modification_id sequence is initialized
schema.config	856	  build_state_puser_i:          IE (personal_user_id)
schema.config	857	  build_state_pmod_i:           IE (personal_modification_id)
schema.config	858	  build_state_rem_queue_time_i: IE (remove_from_queue_time)
schema.config	859	
schema.config	860	
schema.config	861	running:                        table
schema.config	862	
schema.config	863	  build_id:                     build_id M       serial defines
schema.config	864	  agent_id:                     agent_id                refers
schema.config	865	  build_type_id:                bt_int_id               refers
schema.config	866	  build_start_time_agent:       long_int
schema.config	867	  build_start_time_server:      long_int
schema.config	868	  build_finish_time_server:     long_int
schema.config	869	  last_build_activity_time:     long_int
schema.config	870	  is_personal:                  int
schema.config	871	  build_number:                 uni_str(512)
schema.config	872	  build_counter:                long_int
schema.config	873	  requestor:                    str(1024)
schema.config	874	  access_code:                  str(60)
schema.config	875	  queued_ag_restr_type_id:      int                             -- agent restrictor type id
schema.config	876	  queued_ag_restr_id:           int                             -- agent restrictor id
schema.config	877	  build_state_id:               promotion_id            refers
schema.config	878	  agent_type_id:                agent_type_id           refers
schema.config	879	  user_status_text:             uni_str(256)
schema.config	880	  progress_text:                uni_str(1024)
schema.config	881	  current_path_text:            long_uni_str(2048)
schema.config	882	
schema.config	883	  running_pk:                   PK (build_id)
schema.config	884	
schema.config	885	  running_state_id:             IE (build_state_id)
schema.config	886	
schema.config	887	history:                        table
schema.config	888	
schema.config	889	  build_id:                     build_id M      serial  defines
schema.config	890	  agent_name:                   uni_str(256)
schema.config	891	  build_type_id:                bt_int_id               refers
schema.config	892	  branch_name:                  uni_str(1024)
schema.config	893	  build_start_time_server:      long_int
schema.config	894	  build_start_time_agent:       long_int
schema.config	895	  build_finish_time_server:     long_int
schema.config	896	  remove_from_queue_time:       long_int
schema.config	897	  queued_time:                  long_int
schema.config	898	  status:                       int
schema.config	899	  status_text:                  uni_str(256)
schema.config	900	  user_status_text:             uni_str(256)
schema.config	901	  pin:                          int
schema.config	902	  is_personal:                  int
schema.config	903	  is_canceled:                  int
schema.config	904	  build_number:                 uni_str(512)
schema.config	905	  requestor:                    str(1024)
schema.config	906	  build_state_id:               promotion_id            refers
schema.config	907	  agent_type_id:                agent_type_id
schema.config	908	
schema.config	909	  history_build_id_pk:          PK (build_id)
schema.config	910	
schema.config	911	  history_start_time_idx:       IE (build_start_time_server)
schema.config	912	  history_finish_time_idx:      IE (build_finish_time_server)
schema.config	913	  history_bt_id_rm_from_q_time: IE (build_type_id, remove_from_queue_time)
schema.config	914	  history_remove_from_q_time_i: IE (remove_from_queue_time)
schema.config	915	  history_build_type_id_i:      IE (build_type_id, branch_name, is_canceled, pin)
schema.config	916	  history_build_state_id_i:     IE (build_state_id)
schema.config	917	  history_agent_finish_time_i:  IE (agent_name, build_finish_time_server)
schema.config	918	  history_build_number_i:       IE (build_number)
schema.config	919	  history_agent_type_b_id_i:    IE (agent_type_id, build_id)
schema.config	920	
schema.config	921	
schema.config	922	removed_builds_history:         table
schema.config	923	
schema.config	924	  build_id:                     build_id M      serial  defines
schema.config	925	  agent_name:                   uni_str(256)
schema.config	926	  build_type_id:                bt_int_id               refers
schema.config	927	  build_start_time_server:      long_int
schema.config	928	  build_start_time_agent:       long_int
schema.config	929	  build_finish_time_server:     long_int
schema.config	930	  status:                       int
schema.config	931	  status_text:                  uni_str(256)
schema.config	932	  user_status_text:             uni_str(256)
schema.config	933	  pin:                          int
schema.config	934	  is_personal:                  int
schema.config	935	  is_canceled:                  int
schema.config	936	  build_number:                 uni_str(512)
schema.config	937	  requestor:                    str(1024)
schema.config	938	  queued_time:                  long_int
schema.config	939	  remove_from_queue_time:       long_int
schema.config	940	  build_state_id:               long_int
schema.config	941	  agent_type_id:                agent_type_id
schema.config	942	  branch_name:                  uni_str(1024)       -- null means no branch (master)
schema.config	943	
schema.config	944	  removed_builds_history_pk:     PK (build_id)
schema.config	945	
schema.config	946	  removed_b_start_time_index:    IE (build_start_time_server)
schema.config	947	  removed_b_history_finish_time: IE (build_finish_time_server)
schema.config	948	  removed_b_stats_optimized_i:   IE (build_type_id, status, is_canceled, branch_name)
schema.config	949	  removed_b_agent_buildid:       IE (agent_type_id, build_id)
schema.config	950	
schema.config	951	
schema.config	952	build_project:                  table
schema.config	953	
schema.config	954	  build_id:                     build_id M        refers
schema.config	955	  project_level:                int M
schema.config	956	  project_int_id:               project_int_id M  refers
schema.config	957	
schema.config	958	  build_project_pk:             PK (build_id, project_level)
schema.config	959	  build_project_project_idx:    IE (project_int_id)
schema.config	960	
schema.config	961	
schema.config	962	build_dependency:               table
schema.config	963	
schema.config	964	  build_state_id:               promotion_id M    refers
schema.config	965	  depends_on:                   promotion_id M    refers
schema.config	966	  dependency_options:           int
schema.config	968	  build_dependency_pk:          PK (build_state_id, depends_on)
schema.config	969	  build_dependency_ak:          IE (depends_on, build_state_id)
schema.config	970	
schema.config	971	
schema.config	972	build_attrs:                    table -- really it's promotion attrs
schema.config	973	
schema.config	974	  build_state_id:               promotion_id M    refers
schema.config	975	  attr_name:                    str(70) M
schema.config	976	  attr_value:                   uni_str(1000)
schema.config	977	  attr_num_value:               long_int
schema.config	978	
schema.config	979	  build_attrs_pk:               PK (build_state_id, attr_name)
schema.config	980	  build_attrs_num_i:            IE (attr_num_value, attr_name, build_state_id)
schema.config	981	  build_attrs_name_idx:         IE (attr_name)
schema.config	982	
schema.config	983	
schema.config	984	build_data_storage:             table
schema.config	985	
schema.config	986	  build_id:                     build_id M              refers
schema.config	987	  metric_id:                    metric_hash M           refers
schema.config	988	  metric_value:                 decimal(19,6) M
schema.config	989	
schema.config	990	  build_data_storage_pk:        PK (build_id, metric_id)
schema.config	991	
schema.config	992	
schema.config	993	canceled_info:                  table
schema.config	994	
schema.config	995	  build_id:                     build_id M              refers
schema.config	996	  user_id:                      user_id                 refers
schema.config	997	  description:                  str(256)
schema.config	998	  interrupt_type:               int
schema.config	999	
schema.config	1000	  canceled_info_pk:             PK (build_id)
schema.config	1001	
schema.config	1002	failed_tests:                   table
schema.config	1003	
schema.config	1004	  test_name_id:                 test_name_hash M refers
schema.config	1005	  build_id:                     build_id M       refers
schema.config	1006	  test_id:                      int M
schema.config	1007	  ffi_build_id:                 build_id          -- null -not calculated, current build_id - first failure, other build_id > 0 - known FFI, -1 - unknown
schema.config	1008	
schema.config	1009	  failed_tests_pk2:             PK (test_name_id, build_id)
schema.config	1010	  failed_tests_build_idx2:      IE (build_id)
schema.config	1011	  failed_tests_ffi_build_idx:   IE (ffi_build_id)
schema.config	1012	
schema.config	1013	test_info:                      table
schema.config	1014	
schema.config	1015	  build_id:                     build_id M       refers
schema.config	1016	  test_id:                      int M
schema.config	1017	  test_name_id:                 test_name_hash   refers
schema.config	1018	  status:                       int
schema.config	1019	  duration:                     int M default 0
schema.config	1020	
schema.config	1021	  test_info_pk:                 PK (build_id, test_id)
schema.config	1022	  test_name_id_idx:             IE (test_name_id)
schema.config	1023	
schema.config	1024	test_info_archive:              table
schema.config	1025	
schema.config	1026	  build_id:                     build_id M       refers
schema.config	1027	  test_id:                      int M
schema.config	1028	  test_name_id:                 test_name_hash M refers
schema.config	1029	  status:                       int
schema.config	1030	  duration:                     int M default 0
schema.config	1031	
schema.config	1032	  test_info_archive_pk:         PK (build_id, test_id)
schema.config	1033	  test_archive_name_id_idx:     IE (test_name_id)
schema.config	1034	
schema.config	1035	test_metadata_dict:             dictionary table
schema.config	1036	
schema.config	1037	  key_id:                       test_metadata_key M defines
schema.config	1038	  name_digest:                  str(32) M
schema.config	1039	  name:                         uni_str(512) M
schema.config	1040	
schema.config	1041	  test_metadata_dict_pk:        PK (key_id) stable
schema.config	1042	  test_metadata_dict_ak:        AK (name_digest)
schema.config	1043	
schema.config	1044	test_metadata_types:            dictionary  table
schema.config	1045	
schema.config	1046	  type_id:                      test_metadata_type_key M defines
schema.config	1047	  name:                         str(64) M
schema.config	1048	
schema.config	1049	  test_metadata_types_pk:       PK (type_id) stable
schema.config	1050	  test_metadata_types_ak:       AK (name)
schema.config	1051	
schema.config	1052	test_metadata:                  table
schema.config	1053	
schema.config	1054	  build_id:                     build_id M       refers
schema.config	1055	  test_id:                      int M
schema.config	1056	  test_name_id:                 test_name_hash M refers
schema.config	1057	  key_id:                       test_metadata_key M refers
schema.config	1058	  type_id:                      test_metadata_type_key refers
schema.config	1059	  str_value:                    uni_str(1024)
schema.config	1060	  num_value:                    decimal(19,6)
schema.config	1062	  test_metadata_pk:             PK (build_id, test_id, key_id)
schema.config	1063	  test_metadataname_name_idx:   IE (test_name_id)
schema.config	1064	
schema.config	1065	
schema.config	1066	build_problem:                  table
schema.config	1067	
schema.config	1068	  build_state_id:               promotion_id M    refers
schema.config	1069	  problem_id:                   problem_id M      refers
schema.config	1070	  problem_description:          long_str(4000)
schema.config	1071	
schema.config	1072	  build_problem_pk:             PK (build_state_id, problem_id)
schema.config	1073	  build_problem_id_idx:         IE (problem_id)
schema.config	1074	
schema.config	1075	
schema.config	1076	build_problem_attribute:        table
schema.config	1077	
schema.config	1078	  build_state_id:               promotion_id M    refers
schema.config	1079	  problem_id:                   problem_id M      refers
schema.config	1080	  attr_name:                    str(60) M
schema.config	1081	  attr_value:                   str(2000) M
schema.config	1082	
schema.config	1083	  build_problem_attribute_pk:   PK (build_state_id, problem_id, attr_name)
schema.config	1084	  build_problem_attr_p_id_idx:  IE (problem_id)
schema.config	1085	
schema.config	1086	
schema.config	1087	build_artifact_dependency:      table
schema.config	1088	
schema.config	1089	  artif_dep_id:                 dependency_id M
schema.config	1090	  build_state_id:               promotion_id      refers
schema.config	1091	  source_build_type_id:         bt_int_id         refers
schema.config	1092	  revision_rule:                str(80)
schema.config	1093	  branch:                       str(255)
schema.config	1094	  src_paths:                    long_str(40960)
schema.config	1095	
schema.config	1096	  build_artif_dep_state_id:     IE (build_state_id)
schema.config	1097	
schema.config	1098	
schema.config	1099	final_artifact_dependency:      table
schema.config	1100	
schema.config	1101	  artif_dep_id:                 dependency_id M
schema.config	1102	  build_state_id:               promotion_id      refers
schema.config	1103	  source_build_type_id:         bt_int_id         refers
schema.config	1104	  revision_rule:                str(80)
schema.config	1105	  branch:                       str(255)
schema.config	1106	  src_paths:                    long_str(40960)
schema.config	1107	
schema.config	1108	  final_artif_dep_state_id:     IE (build_state_id)
schema.config	1109	  final_artif_dep_src_bt_id:    IE (source_build_type_id)
schema.config	1110	
schema.config	1111	
schema.config	1112	build_type_vcs_change:          table
schema.config	1113	
schema.config	1114	  modification_id:              modification_id M   refers
schema.config	1115	  build_type_id:                bt_int_id M         refers
schema.config	1116	  change_type:                  int
schema.config	1117	
schema.config	1118	  build_type_vcs_change_ui:     AK (modification_id, build_type_id)
schema.config	1119	  build_type_vcs_change_btid:   IE (build_type_id)
schema.config	1120	
schema.config	1121	
schema.config	1122	build_type_edge_relation:       table
schema.config	1123	
schema.config	1124	  child_modification_id:        modification_id M   refers
schema.config	1125	  build_type_id:                bt_int_id M         refers
schema.config	1126	  parent_num:                   int M
schema.config	1127	  change_type:                  int
schema.config	1128	
schema.config	1129	  build_type_edge_relation_pk:  PK (child_modification_id, build_type_id, parent_num)
schema.config	1130	  bt_edge_relation_btid:        IE (build_type_id)
schema.config	1131	
schema.config	1132	
schema.config	1133	ids_group:                      table
schema.config	1134	
schema.config	1135	  id:                           ids_group_id M    defines
schema.config	1136	  group_hash:                   str(80) M
schema.config	1137	
schema.config	1138	  ids_group_pk:                 PK (id)
schema.config	1139	  ids_group_hash_idx:           IE (group_hash)
schema.config	1140	
schema.config	1141	
schema.config	1142	ids_group_entity_id:            table
schema.config	1143	
schema.config	1144	  group_id:                     ids_group_id M       refers
schema.config	1145	  entity_id:                    str(160) M
schema.config	1146	
schema.config	1147	  ids_group_idx:                IE (group_id, entity_id)
schema.config	1148	  ids_group_entity_id_idx:      IE (entity_id)
schema.config	1149	
schema.config	1150	
schema.config	1151	build_type_group_vcs_change:    table
schema.config	1152	
schema.config	1153	  modification_id:              modification_id M    refers
schema.config	1154	  group_id:                     ids_group_id M       refers
schema.config	1155	  change_type:                  int
schema.config	1156	
schema.config	1157	  bt_grp_vcs_change_mod_idx:     IE (modification_id)
schema.config	1158	  bt_grp_vcs_change_grp_mod_idx: IE (group_id, modification_id)
schema.config	1159	
schema.config	1160	build_checkout_rules:           table
schema.config	1161	
schema.config	1162	  build_state_id:               promotion_id M           refers
schema.config	1163	  vcs_root_id:                  vcs_root_instance_id M   refers
schema.config	1164	  checkout_rules:               long_uni_str(16000)
schema.config	1165	
schema.config	1166	  build_checkout_rules_vid_pk:  PK (build_state_id, vcs_root_id)
schema.config	1167	
schema.config	1168	
schema.config	1169	mute_info:                      table
schema.config	1170	
schema.config	1171	  -- invariant records (they're expected to be immutable)
schema.config	1172	  mute_id:                      mute_id M         serial defines
schema.config	1173	  muting_user_id:               user_id M         refers
schema.config	1174	  muting_time:                  timestamp M
schema.config	1175	  muting_comment:               uni_str(2000)
schema.config	1176	  scope:                        char M                       -- possible values: B (build), C (configuration), P (project)
schema.config	1177	  project_int_id:               project_int_id M  refers
schema.config	1178	  build_id:                     build_id          refers
schema.config	1179	  unmute_when_fixed:            boolean
schema.config	1180	  unmute_by_time:               timestamp
schema.config	1181	
schema.config	1182	  mute_info_pk:                 PK (mute_id)
schema.config	1183	  mute_info_ak:                 AK (project_int_id, mute_id)
schema.config	1184	
schema.config	1185	
schema.config	1186	mute_info_bt:                   table
schema.config	1187	
schema.config	1188	  -- build type was muted (a detail of the mute_info), not currently muted
schema.config	1189	  mute_id:                      mute_id M         refers
schema.config	1190	  build_type_id:                bt_int_id M       refers
schema.config	1191	
schema.config	1192	  mute_info_bt_pk:              PK (mute_id, build_type_id)
schema.config	1193	  mute_info_bt_ie:              IE (build_type_id)
schema.config	1194	
schema.config	1195	
schema.config	1196	mute_info_test:                 table
schema.config	1197	
schema.config	1198	  -- test was muted (a detail of the mute_info), not currently muted
schema.config	1199	  mute_id:                      mute_id M         refers
schema.config	1200	  test_name_id:                 test_name_hash M  refers
schema.config	1201	
schema.config	1202	  mute_info_test_pk:            PK (mute_id, test_name_id)
schema.config	1203	
schema.config	1204	
schema.config	1205	mute_test_in_proj:              table
schema.config	1206	
schema.config	1207	  -- currently muted test in project
schema.config	1208	  mute_id:                      mute_id M         refers   -- records can be reassigned from one muting to another
schema.config	1209	  project_int_id:               project_int_id M  refers
schema.config	1210	  test_name_id:                 test_name_hash M  refers
schema.config	1211	
schema.config	1212	  mute_test_in_proj_pk:         PK (mute_id, project_int_id, test_name_id)
schema.config	1213	  mute_test_in_proj_ie:         IE (project_int_id, test_name_id, mute_id)
schema.config	1214	  mute_test_in_proj_tn_ie:      IE (test_name_id)
schema.config	1215	
schema.config	1216	
schema.config	1217	mute_test_in_bt:                table
schema.config	1218	
schema.config	1219	  -- currently muted test in build configuration
schema.config	1220	  mute_id:                      mute_id M         refers   -- records can be reassigned from one muting to another
schema.config	1221	  build_type_id:                bt_int_id M       refers
schema.config	1222	  test_name_id:                 test_name_hash M  refers
schema.config	1223	
schema.config	1224	  mute_test_in_bt_pk:           PK (mute_id, build_type_id, test_name_id)
schema.config	1225	  mute_test_in_bt_ie:           IE (build_type_id, test_name_id, mute_id)
schema.config	1226	  mute_test_in_bt_tn_ie:        IE (test_name_id)
schema.config	1227	
schema.config	1228	
schema.config	1229	mute_info_problem:              table
schema.config	1230	
schema.config	1231	  mute_id:                      mute_id M         refers
schema.config	1232	  problem_id:                   problem_id M      refers
schema.config	1233	
schema.config	1234	  mute_info_problem_pk:         PK (mute_id, problem_id)
schema.config	1235	
schema.config	1236	
schema.config	1237	mute_problem_in_proj:           table
schema.config	1238	
schema.config	1239	  -- currently muted build problem in project
schema.config	1240	  mute_id:                      mute_id M         refers
schema.config	1241	  project_int_id:               project_int_id M  refers
schema.config	1242	  problem_id:                   problem_id M      refers
schema.config	1243	
schema.config	1244	  mute_problem_in_proj_pk:      PK (mute_id, project_int_id, problem_id)
schema.config	1245	  mute_problem_in_proj_ie:      IE (project_int_id, problem_id, mute_id)
schema.config	1246	
schema.config	1247	
schema.config	1248	mute_problem_in_bt:             table
schema.config	1249	
schema.config	1250	  -- currently muted build problem in build configuration
schema.config	1251	  mute_id:                      mute_id M         refers
schema.config	1252	  build_type_id:                bt_int_id M       refers
schema.config	1253	  problem_id:                   problem_id M      refers
schema.config	1254	
schema.config	1255	  mute_problem_in_bt_pk:        PK (mute_id, build_type_id, problem_id)
schema.config	1256	  mute_problem_in_bt_ie:        IE (build_type_id, problem_id, mute_id)
schema.config	1257	
schema.config	1258	
schema.config	1259	build_problem_muted:            table
schema.config	1260	
schema.config	1261	  build_state_id:               promotion_id M    refers
schema.config	1262	  problem_id:                   problem_id M      refers
schema.config	1263	  mute_id:                      mute_id           refers   -- may be null if problem internally muted during the build
schema.config	1264	
schema.config	1265	  build_problem_muted_pk:       PK (build_state_id, problem_id)
schema.config	1266	  build_problem_mute_id:        IE (mute_id)
schema.config	1267	
schema.config	1268	
schema.config	1269	test_muted:                     table
schema.config	1270	
schema.config	1271	  build_id:                     build_id M         refers
schema.config	1272	  test_name_id:                 test_name_hash M   refers
schema.config	1273	  mute_id:                      mute_id M          refers
schema.config	1274	
schema.config	1275	  test_muted_pk:                PK (build_id, test_name_id, mute_id)
schema.config	1276	  test_muted_mute_id:           IE (mute_id)
schema.config	1277	
schema.config	1278	
schema.config	1279	test_failure_rate:              table
schema.config	1280	
schema.config	1281	  build_type_id:                bt_int_id M        refers
schema.config	1282	  test_name_id:                 test_name_hash M   refers
schema.config	1283	  success_count:                int
schema.config	1284	  failure_count:                int
schema.config	1285	  last_failure_time:            long_int
schema.config	1286	
schema.config	1287	  test_failure_rate_pk:         PK (build_type_id, test_name_id)
schema.config	1288	  test_failure_rate_tn_idx:     IE (test_name_id)
schema.config	1289	
schema.config	1290	
schema.config	1291	build_queue:                    table
schema.config	1292	
schema.config	1293	  build_type_id:                bt_int_id          refers
schema.config	1294	  agent_restrictor_type_id:     int
schema.config	1295	  agent_restrictor_id:          int
schema.config	1296	  requestor:                    str(1024)
schema.config	1297	  build_state_id:               promotion_id       refers
schema.config	1298	
schema.config	1299	  build_queue_build_state_id:   IE (build_state_id)
schema.config	1300	
schema.config	1301	build_queue_order:              table
schema.config	1302	
schema.config	1303	  version:                      long_int M
schema.config	1304	  line_num:                     int M
schema.config	1305	  promotion_ids:                str(2000)
schema.config	1306	
schema.config	1307	  build_queue_order_pk:         PK (version, line_num)
schema.config	1308	
schema.config	1309	
schema.config	1310	stats:                          table
schema.config	1311	
schema.config	1312	  build_id:                     build_id M         refers
schema.config	1313	  test_count:                   int
schema.config	1314	  -- a record per build
schema.config	1315	
schema.config	1316	  stats_pk:                     PK (build_id)
schema.config	1317	
schema.config	1318	
schema.config	1319	failed_tests_output:            table
schema.config	1320	
schema.config	1321	  build_id:                     build_id M         refers
schema.config	1322	  test_id:                      int M
schema.config	1323	  problem_description:          long_str(256)
schema.config	1324	  std_output:                   long_str(40960)
schema.config	1325	  error_output:                 long_str(40960)
schema.config	1326	  stacktrace:                   long_str(40960)
schema.config	1327	  expected:                     long_str(40960)
schema.config	1328	  actual:                       long_str(40960)
schema.config	1329	
schema.config	1330	  failed_tests_output_pk:       PK (build_id, test_id)
schema.config	1331	
schema.config	1332	
schema.config	1333	compiler_output:                table
schema.config	1334	
schema.config	1335	  build_id:                     build_id           refers
schema.config	1336	  message_order:                int
schema.config	1337	  message:                      long_str(40960)
schema.config	1338	
schema.config	1339	  co_build_id_index:            IE (build_id)
schema.config	1340	
schema.config	1341	
schema.config	1342	ignored_tests:                  table
schema.config	1343	
schema.config	1344	  build_id:                     build_id           refers
schema.config	1345	  test_id:                      int
schema.config	1346	  ignore_reason:                uni_str(2000)
schema.config	1347	
schema.config	1348	  ignored_tests_build_id:       IE (build_id)
schema.config	1349	
schema.config	1350	
schema.config	1351	downloaded_artifacts:           table
schema.config	1352	
schema.config	1353	  target_build_id:              build_id           refers   -- artifact was downloaded to
schema.config	1354	  source_build_id:              build_id           refers   -- artifact was downloaded from
schema.config	1355	  download_timestamp:           long_int
schema.config	1356	  artifact_path:                long_str(8192)
schema.config	1357	
schema.config	1358	  downloaded_artifacts_source_id: IE (source_build_id)
schema.config	1359	  downloaded_artifacts_ts_id: IE (target_build_id,source_build_id)
schema.config	1360	
schema.config	1361	
schema.config	1362	build_revisions:                table
schema.config	1363	
schema.config	1364	  build_state_id:               promotion_id M          refers
schema.config	1365	  vcs_root_id:                  vcs_root_instance_id M  refers
schema.config	1366	  vcs_revision:                 vcs_revision M
schema.config	1367	  vcs_revision_display_name:    str(200)
schema.config	1368	  vcs_branch_name:              uni_str(1024)
schema.config	1369	  modification_id:              modification_id         refers
schema.config	1370	  vcs_root_type:                int
schema.config	1371	  checkout_mode:                int  -- null means the default checkout mode from a buildType, see BuildRevisionCheckoutMode
schema.config	1372	
schema.config	1373	  build_revisions_pk:           PK (build_state_id, vcs_root_id)
schema.config	1374	  build_revisions_vcs_root_i:   IE (vcs_root_id)
schema.config	1375	  build_revisions_mod_id_i:     IE (modification_id)    -- see TW-47662
schema.config	1376	
schema.config	1377	
schema.config	1378	default_build_parameters:       table
schema.config	1379	
schema.config	1380	  build_state_id:               promotion_id       refers
schema.config	1381	  param_name:                   str(2000)
schema.config	1382	  param_value:                  long_uni_str(16000)
schema.config	1383	
schema.config	1384	  def_build_params_state_id:    IE (build_state_id)
schema.config	1385	
schema.config	1386	
schema.config	1387	user_build_parameters:          table
schema.config	1388	
schema.config	1389	  build_state_id:               promotion_id       refers
schema.config	1390	  param_name:                   str(2000)
schema.config	1391	  param_value:                  long_uni_str(16000)
schema.config	1392	
schema.config	1393	  user_build_params_state_id:   IE (build_state_id)
schema.config	1394	
schema.config	1395	
schema.config	1396	build_labels:                   table
schema.config	1397	
schema.config	1398	  build_id:                     build_id M              refers
schema.config	1399	  vcs_root_id:                  vcs_root_instance_id M  refers
schema.config	1400	  label:                        str(80)
schema.config	1401	  status:                       int default 0
schema.config	1402	  error_message:                str(256)
schema.config	1403	
schema.config	1404	  build_labels_pk:              PK (build_id, vcs_root_id)
schema.config	1405	  build_labels_vcs_root_i:      IE (vcs_root_id)
schema.config	1406	
schema.config	1407	
schema.config	1408	personal_build_relative_path:   table
schema.config	1409	
schema.config	1410	  build_id:                     build_id           refers
schema.config	1411	  original_path_hash:           long_int
schema.config	1412	  relative_path:                long_str(16000)
schema.config	1413	
schema.config	1414	  personal_build_relative_p_ak: AK (build_id, original_path_hash)
schema.config	1415	
schema.config	1416	
schema.config	1417	responsibilities:               table
schema.config	1418	
schema.config	1419	  problem_id:                   str(80) M          -- it's not our problem_id, it's something else
schema.config	1420	  state:                        int M
schema.config	1421	  responsible_user_id:          user_id M          refers
schema.config	1422	  reporter_user_id:             user_id            refers
schema.config	1423	  timestmp:                     long_int
schema.config	1424	  remove_method:                int M default 0
schema.config	1425	  comments:                     long_uni_str(4096)
schema.config	1426	
schema.config	1427	  responsibilities_pk:          PK (problem_id)
schema.config	1428	
schema.config	1429	  responsibilities_reporter:    IE (reporter_user_id)
schema.config	1430	  responsibilities_assignee:    IE (responsible_user_id)
schema.config	1431	
schema.config	1432	
schema.config	1433	build_state_tag:                table
schema.config	1434	
schema.config	1435	  build_state_id:               promotion_id M     refers
schema.config	1436	  tag:                          tag_phrase M
schema.config	1437	
schema.config	1438	  build_state_tag_pk:           PK (build_state_id, tag)
schema.config	1439	  build_state_tag_ie1:          IE (tag, build_state_id)
schema.config	1440	
schema.config	1441	
schema.config	1442	build_state_private_tag:        table
schema.config	1443	
schema.config	1444	  build_state_id:               promotion_id M     refers
schema.config	1643	
schema.config	1445	  owner:                        user_id M          refers
schema.config	1446	  tag:                          tag_phrase M
schema.config	1447	
schema.config	1448	  build_state_private_tag_pk:   PK (build_state_id, owner, tag)
schema.config	1449	  build_state_private_tag_ie1:  IE (owner, build_state_id)
schema.config	1450	
schema.config	1451	
schema.config	1452	build_overriden_roots:          table
schema.config	1453	
schema.config	1454	  build_state_id:               promotion_id                refers
schema.config	1455	  original_vcs_root_id:         vcs_root_instance_id M      refers
schema.config	1456	  substitution_vcs_root_id:     vcs_root_instance_id M      refers
schema.config	1457	
schema.config	1458	  build_overriden_roots_pk:     PK (build_state_id, original_vcs_root_id)
schema.config	1459	  build_subst_root_index:       IE (substitution_vcs_root_id)
schema.config	1460	  build_orig_root_index:        IE (original_vcs_root_id)
schema.config	1461	
schema.config	1462	
schema.config	1463	user_roles:                     table
schema.config	1464	
schema.config	1465	  user_id:                      user_id M       refers
schema.config	1466	  role_id:                      user_role_id M  -- we need a dictionary of roles
schema.config	1467	  project_int_id:               project_int_id  refers
schema.config	1468	
schema.config	1469	  user_roles_ui:                AK (user_id, role_id, project_int_id)
schema.config	1470	  user_roles_p_int_id_i:        IE (project_int_id)
schema.config	1471	
schema.config	1472	
schema.config	1473	usergroup_roles:                table
schema.config	1474	
schema.config	1475	  group_id:                     group_id M      refers
schema.config	1476	  role_id:                      user_role_id M  -- we need a dictionary of roles
schema.config	1477	  project_int_id:               project_int_id  refers
schema.config	1478	
schema.config	1479	  usergroup_roles_ui:           AK (group_id, role_id, project_int_id)
schema.config	1480	  usergroup_roles_p_int_id_i:   IE (project_int_id)
schema.config	1481	
schema.config	1482	
schema.config	1483	
schema.config	1484	-- INSPECTIONS AND DUPLICATES
schema.config	1485	
schema.config	1486	inspection_info:                dictionary table
schema.config	1487	
schema.config	1488	  id:                           inspection_id_hash M     defines
schema.config	1489	  inspection_id:                inspection_id_str        defines
schema.config	1490	  inspection_name:              str(255)
schema.config	1491	  inspection_desc:              long_str(4000)
schema.config	1492	  group_name:                   str(255)
schema.config	1493	
schema.config	1494	  inspection_info_pk:           PK (id)                  stable
schema.config	1495	  inspection_info_ak:           AK (inspection_id)       stable
schema.config	1496	
schema.config	1497	
schema.config	1498	inspection_data:                dictionary table
schema.config	1499	
schema.config	1500	  hash:                         inspection_data_hash M   defines
schema.config	1501	  result:                       long_str(4000)
schema.config	1502	  severity:                     int
schema.config	1503	  type_pattern:                 int
schema.config	1504	  fqname:                       long_str(4000)
schema.config	1505	  file_name:                    str(255)
schema.config	1506	  parent_fqnames:               long_str(4000)
schema.config	1507	  parent_type_patterns:         str(20)
schema.config	1508	  module_name:                  str(40)
schema.config	1509	  inspection_id:                inspection_id_hash       refers
schema.config	1510	  is_local:                     int
schema.config	1511	  used:                         int M default 1
schema.config	1512	
schema.config	1513	  inspection_data_pk:           PK (hash)                stable
schema.config	1514	
schema.config	1515	  inspection_data_file_index:   IE (file_name)
schema.config	1516	  inspection_data_insp_index:   IE (inspection_id)
schema.config	1517	
schema.config	1518	
schema.config	1519	inspection_fixes:               table
schema.config	1520	
schema.config	1521	  hash:                         inspection_data_hash M   refers
schema.config	1522	  hint:                         str(255)
schema.config	1523	
schema.config	1524	  inspection_fixes_hash_index:  IE (hash)
schema.config	1525	
schema.config	1526	
schema.config	1527	inspection_results:             table
schema.config	1528	
schema.config	1529	  build_id:                     build_id M               refers
schema.config	1530	  hash:                         inspection_data_hash M   refers
schema.config	1531	  line:                         int M
schema.config	1532	
schema.config	1533	  inspection_results_hash_index:   IE (hash)
schema.config	1534	  inspection_results_buildhash_i:  IE (build_id, hash)
schema.config	1535	
schema.config	1536	
schema.config	1537	inspection_stats:               table
schema.config	1538	
schema.config	1539	  build_id:                     build_id M               refers
schema.config	1540	  total:                        int
schema.config	1541	  new_total:                    int
schema.config	1542	  old_total:                    int
schema.config	1543	  errors:                       int
schema.config	1544	  new_errors:                   int
schema.config	1545	  old_errors:                   int
schema.config	1546	
schema.config	1547	  inspection_stats_pk:          PK (build_id)
schema.config	1548	
schema.config	1549	
schema.config	1550	inspection_diff:                table
schema.config	1551	
schema.config	1552	  build_id:                     build_id M               refers
schema.config	1553	  hash:                         inspection_data_hash M   refers
schema.config	1554	
schema.config	1555	  inspection_diff_ak:           AK (build_id, hash)
schema.config	1556	
schema.config	1557	  inspection_diff_hash_index:   IE (hash)
schema.config	1558	
schema.config	1559	
schema.config	1560	project_files:                  dictionary table
schema.config	1561	
schema.config	1562	  file_id:                      duplicate_file_id M serial defines
schema.config	1563	  file_name:                    duplicate_file_name M
schema.config	1564	
schema.config	1565	  project_files_pk:             PK (file_id)
schema.config	1566	  project_files_ak:             AK (file_name) stable
schema.config	1567	
schema.config	1568	
schema.config	1569	duplicate_results:              table
schema.config	1570	
schema.config	1571	  id:                           duplicate_result_id M serial defines
schema.config	1572	  build_id:                     build_id M refers
schema.config	1573	  hash:                         duplicate_result_hash M defines
schema.config	1574	  cost:                         int
schema.config	1575	
schema.config	1576	  duplicate_results_pk:         PK (id)
schema.config	1577	  duplicate_results_build_i:    IE (build_id)
schema.config	1578	
schema.config	1579	
schema.config	1580	duplicate_diff:                 table
schema.config	1581	
schema.config	1582	  build_id:                     build_id M refers
schema.config	1583	  hash:                         long_int M
schema.config	1584	
schema.config	1585	  duplicate_diff_pk:            PK (build_id, hash)
schema.config	1586	
schema.config	1587	
schema.config	1588	duplicate_fragments:            table
schema.config	1589	
schema.config	1590	  id:                           duplicate_result_id M  refers
schema.config	1591	  file_id:                      duplicate_file_id   M  refers
schema.config	1592	  line:                         int                 M
schema.config	1593	  offset_info:                  str(100)            M
schema.config	1594	
schema.config	1595	  duplicate_fragments_pk:       PK (id, file_id, line, offset_info)
schema.config	1596	  duplicate_fragments_file_i:   IE (file_id)
schema.config	1597	
schema.config	1598	
schema.config	1599	duplicate_stats:                table
schema.config	1600	
schema.config	1601	  build_id:                     build_id M refers
schema.config	1602	  total:                        int
schema.config	1603	  new_total:                    int
schema.config	1604	  old_total:                    int
schema.config	1605	
schema.config	1606	  duplicate_stats_pk:           PK (build_id)
schema.config	1607	
schema.config	1608	
schema.config	1609	
schema.config	1610	
schema.config	1611	-- OTHER TABLES
schema.config	1612	
schema.config	1613	stats_publisher_state:          table
schema.config	1614	
schema.config	1615	  metric_id:                    long_int M
schema.config	1616	  value:                        long_int M
schema.config	1617	
schema.config	1618	  stats_publisher_state_pk:     PK (metric_id)
schema.config	1619	
schema.config	1620	
schema.config	1621	comments:                       table
schema.config	1622	
schema.config	1623	  id:                           audit_comment_id M  serial defines
schema.config	1624	  author_id:                    user_id             refers
schema.config	1625	  when_changed:                 long_int  M
schema.config	1626	  commentary:                   long_uni_str(4096)
schema.config	1627	
schema.config	1628	  comments_pk:                  PK (id)
schema.config	1629	  comments_when_changed_i:      IE (when_changed)
schema.config	1630	
schema.config	1631	
schema.config	1632	action_history:                 table
schema.config	1633	
schema.config	1634	  object_id:                    str(80)
schema.config	1635	  comment_id:                   audit_comment_id    refers
schema.config	1636	  action:                       int
schema.config	1637	  additional_data:              str(80)
schema.config	1638	
schema.config	1639	  action_history_comment:          IE (comment_id)
schema.config	1640	  action_history_object:           IE (object_id)
schema.config	1641	  action_history_action_object_i:  IE (action, object_id)
schema.config	1642	
schema.config	1644	audit_additional_object:        table
schema.config	1645	
schema.config	1646	  comment_id:                   audit_comment_id    refers
schema.config	1647	  object_index:                 int
schema.config	1648	  object_id:                    str(80)
schema.config	1649	  object_name:                  long_uni_str(2500) -- is used only for deleted objects
schema.config	1650	
schema.config	1651	  audit_a_o_comment:            IE (comment_id)
schema.config	1652	
schema.config	1653	
schema.config	1654	build_set_tmp:                  table
schema.config	1655	
schema.config	1656	  build_id:                     build_id M
schema.config	1657	
schema.config	1658	  build_set_pk:                 PK (build_id)
schema.config	1659	
schema.config	1660	
schema.config	1661	clean_checkout_enforcement:     table
schema.config	1662	
schema.config	1663	  build_type_id:                bt_int_id M
schema.config	1664	  agent_id:                     agent_id M
schema.config	1665	  current_build_id:             build_id M
schema.config	1666	  request_time:                 timestamp M
schema.config	1667	
schema.config	1668	  clean_checkout_enforcement_pk: PK (build_type_id, agent_id)
schema.config	1669	
schema.config	1670	server_statistics:              table
schema.config	1671	  metric_key:                   long_int M
schema.config	1672	  metric_value:                 long_int M
schema.config	1673	  metric_timestamp:             long_int M
schema.config	1674	
schema.config	1675	  metric_key_index:             IE (metric_key, metric_timestamp)
schema.config	1676	
schema.config	1677	node_events:                    table
schema.config	1678	  id:                           long_int M serial defines
schema.config	1679	  name:                         str(64)
schema.config	1680	  long_arg1:                    long_int
schema.config	1681	  long_arg2:                    long_int
schema.config	1682	  str_arg:                      str(255)
schema.config	1683	  node_id:                      str(80) M
schema.config	1684	  created:                      timestamp
schema.config	1685	
schema.config	1686	  node_events_pk:               PK (node_id, id)
schema.config	1687	  node_events_created_idx:      IE(created)
schema.config	1688	
schema.config	1689	
schema.config	1690	node_tasks:                     table
schema.config	1691	  id:                           node_task_id M
schema.config	1692	  task_type:                    str(64) M
schema.config	1693	  task_identity:                str(255) M
schema.config	1694	  long_arg1:                    long_int
schema.config	1695	  long_arg2:                    long_int
schema.config	1696	  str_arg:                      uni_str(1024)
schema.config	1697	  long_str_arg_uuid:            str(40)
schema.config	1698	  node_id:                      str(80) M
schema.config	1699	  executor_node_id:             str(80)
schema.config	1700	  task_state:                   int M
schema.config	1701	  result:                       uni_str(1024)
schema.config	1702	  long_result_uuid:             str(40)
schema.config	1703	  created:                      timestamp
schema.config	1704	  finished:                     timestamp
schema.config	1705	  last_activity:                timestamp
schema.config	1706	
schema.config	1707	  node_tasks_pk:                PK (id)
schema.config	1708	  node_tasks_type_ident_ak:     AK (task_type, task_identity, task_state)
schema.config	1709	  node_tasks_task_state_idx:    IE (task_state)
schema.config	1710	
schema.config	1711	
schema.config	1712	node_tasks_long_value:
schema.config	1713	
schema.config	1714	  uuid:                        str(40) M
schema.config	1715	  long_value:                  long_uni_str(65536) M
schema.config	1716	
schema.config	1717	  node_tasks_long_value_pk:    PK(uuid)
schema.config	1718	
schema.config	1719	
schema.config	1720	node_locks:                    table
schema.config	1721	  lock_type:                   str(64) M
schema.config	1722	  lock_arg:                    str(80)
schema.config	1723	  id:                          long_int M
schema.config	1724	  node_id:                     str(80) M
schema.config	1725	  state:                       int M
schema.config	1726	  created:                     long_int M
schema.config	1727	
schema.config	1728	  node_locks_pk:               PK (lock_type, id)
schema.config	1729	  node_locks_node_id_idx:      IE (node_id, id, lock_type)
schema.config	1730	
schema.config	1731	
schema.config	1732	custom_data_body:               table
schema.config	1733	  id:                           custom_data_body_id M serial defines -- id of custom data body
schema.config	1734	  part_num:                     int M              -- body part number (can be > 0 if body length > 2000)
schema.config	1735	  total_parts:                  int M              -- total number of body parts
schema.config	1736	  data_body:                    uni_str(2000)      -- body part
schema.config	1737	  update_date:                  long_int M         -- timestamp when this part was created or updated
schema.config	1738	
schema.config	1739	  custom_data_body_idx:         AK (id, part_num)
schema.config	1740	  custom_data_body_ud_idx:      IE (update_date)
schema.config	1741	
schema.config	1742	custom_data:                    table
schema.config	1743	  data_key_hash:                str(80) M          -- sha1 of concat(data_domain, data_key), we need it because MySQL can't create index for column > 760 chars
schema.config	1744	  collision_idx:                int M              -- used only if we detected sha1 collision
schema.config	1745	  data_domain:                  str(80) M          -- custom data domain: buildType:<id>, project:<id> or vcsRoot:<id>
schema.config	1746	  data_key:                     uni_str(2000) M    -- custom data storage id
schema.config	1747	  data_id:                      custom_data_body_id M  refers   -- id of custom data body
schema.config	1748	
schema.config	1749	  custom_data_key_hash_idx:     AK (data_key_hash, collision_idx)
schema.config	1750	  custom_data_domain_idx:       IE (data_domain)
schema.config	1751	  custom_data_body_id_idx:      IE (data_id)
schema.config	1752	
schema.config	1753	
schema.config	1754	-- SERVER HEALTH TABLES --
schema.config	1755	
schema.config	1756	server_health_items:            table
schema.config	1757	
schema.config	1758	  id:                           server_health_item_id M serial defines
schema.config	1759	  report_id:                    str(80) M
schema.config	1760	  category_id:                  str(80) M
schema.config	1761	  item_id:                      str(255) M
schema.config	1762	
schema.config	1763	  server_health_items_pk:       PK (id)
schema.config	1764	  server_health_items_ie:       IE (report_id, category_id)
schema.config	1765	
schema.config	1766	hidden_health_item:             table
schema.config	1767	
schema.config	1768	  item_id:                      server_health_item_id M    refers
schema.config	1769	  user_id:                      user_id                    refers    -- null if item is invisible for everyone
schema.config	1770	  health_item_id_ie:            IE (item_id)
schema.config	1771	
schema.config	1772	
schema.config	1773	config_persisting_tasks:       table
schema.config	1774	
schema.config	1775	  id:                          long_int M        -- task id, unique within a specific type
schema.config	1776	  task_type:                   str(20) M         -- the type of the tasks
schema.config	1777	  description:                 uni_str(2000)     -- some human friendly description of the task
schema.config	1778	  stage:                       int M
schema.config	1779	  node_id:                     str(80) M         -- id of a node which created this task
schema.config	1780	  created:                     long_int M        -- timestamp when the task was created
schema.config	1781	  updated:                     long_int M default 0      -- timestamp when the task was updated
schema.config	1782	
schema.config	1783	  config_persisting_tasks_pk:  PK (id, task_type)
schema.config	1784	  config_persisting_tasks_ie:  IE (task_type, stage)
schema.config	1785	
schema.config	1786	
schema.config	1787	
schema.config	1788	-- DEPRECATED TABLES (will not be populated anymore)
schema.config	1789	
schema.config	1790	
schema.config	1791	vcs_changes:                    deprecated table
schema.config	1792	
schema.config	1793	  modification_id:              modification_id
schema.config	1794	  change_name:                  str(64)
schema.config	1795	  change_type:                  int
schema.config	1796	  before_revision:              long_str(2048)
schema.config	1797	  after_revision:               long_str(2048)
schema.config	1798	  vcs_file_name:                long_str(16000)
schema.config	1799	  relative_file_name:           long_str(16000)
schema.config	1800	
schema.config	1801	  vcs_changes_index:            IE (modification_id)
schema.config	1802	
schema.config	1803	
schema.config	1804	personal_vcs_changes:           deprecated table
schema.config	1805	
schema.config	1806	  modification_id:              modification_id
schema.config	1807	  change_name:                  str(64)
schema.config	1808	  change_type:                  int
schema.config	1809	  before_revision:              str(2048)
schema.config	1810	  after_revision:               str(2048)
schema.config	1811	  vcs_file_name:                long_str(16000)
schema.config	1812	  relative_file_name:           long_str(16000)
schema.config	1813	
schema.config	1814	  vcs_personal_changes_index:   IE (modification_id)
schema.config	1815	
schema.config	1816	
schema.config	1817	light_history:                  deprecated table
schema.config	1818	
schema.config	1819	  build_id:                     build_id M
schema.config	1820	  agent_name:                   str(80)
schema.config	1821	  build_type_id:                bt_int_id    refers
schema.config	1822	  build_start_time_server:      long_int
schema.config	1823	  build_start_time_agent:       long_int
schema.config	1824	  build_finish_time_server:     long_int
schema.config	1825	  status:                       int
schema.config	1826	  status_text:                  uni_str(256)
schema.config	1827	  user_status_text:             uni_str(256)
schema.config	1828	  pin:                          int
schema.config	1829	  is_personal:                  int
schema.config	1830	  is_canceled:                  int
schema.config	1831	  build_number:                 str(256)
schema.config	1832	  requestor:                    str(1024)
schema.config	1833	  queued_time:                  long_int
schema.config	1834	  remove_from_queue_time:       long_int
schema.config	1835	  build_state_id:               long_int
schema.config	1836	  agent_type_id:                agent_type_id
schema.config	1837	  branch_name:                  str(255)
schema.config	1838	
schema.config	1839	  light_history_pk:              PK (build_id)
schema.config	1840	
schema.config	1841	  start_time_index_light:        IE (build_start_time_server)
schema.config	1842	  light_history_finish_time_i:   IE (build_finish_time_server)
schema.config	1843	  stats_optimized_index:         IE (build_type_id, status, is_canceled, branch_name)
schema.config	1844	  light_history_agt_b_i:         IE (agent_type_id, build_id)
schema.config	1845	
schema.config	1846	
schema.config	1847	-- TEMPORARY TABLES
schema.config	1848	
schema.config	1849	
schema.config	1850	agent_pool$:                    temporary table
schema.config	1851	
schema.config	1852	  agent_pool_id:                agent_pool_id M
schema.config	1853	
schema.config	1854	
schema.config	1855	agent_type$:                    temporary table
schema.config	1856	
schema.config	1857	  agent_type_id:                agent_type_id M
schema.config	1858	
schema.config	1859	
schema.config	1860	project$:                       temporary table
schema.config	1861	
schema.config	1862	  int_id:                       project_int_id M
schema.config	1863	
schema.config	1864	
schema.config	1865	build_type$:                    temporary table
schema.config	1866	
schema.config	1867	  build_type_id:                bt_int_id M
schema.config	1868	
schema.config	1869	
schema.config	1870	vcs_root_instance$:            temporary table
schema.config	1871	
schema.config	1872	  id:                           vcs_root_instance_id M
schema.config	1873	
schema.config	1874	
schema.config	1875	modification$:                  temporary table
schema.config	1876	
schema.config	1877	  modification_id:              modification_id M
schema.config	1878	
schema.config	1879	
schema.config	1880	promotion$:                     temporary table
schema.config	1881	
schema.config	1882	  id:                           promotion_id M   -- build_state_id in other tables
schema.config	1883	
schema.config	1884	
schema.config	1885	build$:                         temporary table
schema.config	1886	
schema.config	1887	  build_id:                     build_id M
schema.config	1888	
schema.config	1889	
schema.config	1890	test$:                          temporary table
schema.config	1891	
schema.config	1892	  test_name_id:                 test_name_hash M
schema.config	1893	
schema.config	1894	
schema.config	1895	test_key$:                      temporary table
schema.config	1896	
schema.config	1897	  test_name_id:                 test_name_hash M
schema.config	1898	  test_key_pk:                  PK (test_name_id)
schema.config	1899	
schema.config	1900	
schema.config	1901	problem$:                       temporary table
schema.config	1902	
schema.config	1903	  problem_id:                   problem_id M
schema.config	1904	
schema.config	1905	
schema.config	1906	branch$:                        temporary table
schema.config	1907	
schema.config	1908	  branch_name:                  uni_str(1024) M
schema.config	1909	
schema.config	1910	
schema.config	1911	audit_object_ids_to_cleanup$:   temporary table
schema.config	1912	
schema.config	1913	  object_id:                    str(80) M
schema.config	1914	
schema.config	1915	
schema.config	1916	audit_comment_ids_to_cleanup$:  temporary table
schema.config	1917	
schema.config	1918	  comment_id:                   audit_comment_id M
schema.config	1919	
schema.config	1920	
schema.config	1921	user$:                          temporary table
schema.config	1922	
schema.config	1923	  user_id:                      user_id M
schema.config	1924	
schema.config	1925	
schema.config	1926	custom_data_body_id$:           temporary table
schema.config	1927	
schema.config	1928	  id:                           long_int M
schema.config	1929	
schema.config	1930	custom_data_domain$:            temporary table
schema.config	1931	
schema.config	1932	  data_domain:                  str(80) M
schema.config	1933	
schema.config	1934	duplicate_file_name$:             temporary table
schema.config	1935	
schema.config	1936	  file_name:                    duplicate_file_name M
schema.config	1937	
schema.config	1938	ids_group$:                     temporary table
schema.config	1939	
schema.config	1940	  group_id:                     ids_group_id M
schema.config	1941	
schema.config	1942	
schema.config	1943	
schema.config	1944	-- THE LAST TABLE
schema.config	1945	
schema.config	1946	server:                         table
schema.config	1947	
schema.config	1948	  server_id:                    long_int
\.


--
-- TOC entry 6357 (class 0 OID 17141)
-- Dependencies: 319
-- Data for Name: mute_info; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.mute_info (mute_id, muting_user_id, muting_time, muting_comment, scope, project_int_id, build_id, unmute_when_fixed, unmute_by_time) FROM stdin;
\.


--
-- TOC entry 6358 (class 0 OID 17150)
-- Dependencies: 320
-- Data for Name: mute_info_bt; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.mute_info_bt (mute_id, build_type_id) FROM stdin;
\.


--
-- TOC entry 6362 (class 0 OID 17175)
-- Dependencies: 324
-- Data for Name: mute_info_problem; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.mute_info_problem (mute_id, problem_id) FROM stdin;
\.


--
-- TOC entry 6359 (class 0 OID 17156)
-- Dependencies: 321
-- Data for Name: mute_info_test; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.mute_info_test (mute_id, test_name_id) FROM stdin;
\.


--
-- TOC entry 6364 (class 0 OID 17186)
-- Dependencies: 326
-- Data for Name: mute_problem_in_bt; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.mute_problem_in_bt (mute_id, build_type_id, problem_id) FROM stdin;
\.


--
-- TOC entry 6363 (class 0 OID 17180)
-- Dependencies: 325
-- Data for Name: mute_problem_in_proj; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.mute_problem_in_proj (mute_id, project_int_id, problem_id) FROM stdin;
\.


--
-- TOC entry 6361 (class 0 OID 17168)
-- Dependencies: 323
-- Data for Name: mute_test_in_bt; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.mute_test_in_bt (mute_id, build_type_id, test_name_id) FROM stdin;
\.


--
-- TOC entry 6360 (class 0 OID 17161)
-- Dependencies: 322
-- Data for Name: mute_test_in_proj; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.mute_test_in_proj (mute_id, project_int_id, test_name_id) FROM stdin;
\.


--
-- TOC entry 6404 (class 0 OID 17437)
-- Dependencies: 366
-- Data for Name: node_events; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.node_events (id, name, long_arg1, long_arg2, str_arg, node_id, created) FROM stdin;
\.


--
-- TOC entry 6407 (class 0 OID 17460)
-- Dependencies: 369
-- Data for Name: node_locks; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.node_locks (lock_type, lock_arg, id, node_id, state, created) FROM stdin;
\.


--
-- TOC entry 6405 (class 0 OID 17443)
-- Dependencies: 367
-- Data for Name: node_tasks; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.node_tasks (id, task_type, task_identity, long_arg1, long_arg2, str_arg, long_str_arg_uuid, node_id, executor_node_id, task_state, result, long_result_uuid, created, finished, last_activity) FROM stdin;
457876	buildChainChangesCollecting	102	102	\N	\N	\N	MAIN_SERVER	MAIN_SERVER	56266586	\N	\N	2024-03-13 00:20:51.740254	2024-03-13 00:20:54.525	2024-03-13 00:20:54.525
457878	publishBuildStatus.buildStarted	publishBuildStatus.buildStarted:102	102	\N	\N	\N	MAIN_SERVER	MAIN_SERVER	61118271	\N	\N	2024-03-13 00:20:54.61965	2024-03-13 00:20:54.635	2024-03-13 00:20:54.635
380441	processingTriggers	processingTriggers-nuget.simple	\N	\N	nuget.simple	\N	MAIN_SERVER	MAIN_SERVER	64764846	\N	\N	2024-03-10 13:17:10.222206	2024-03-12 23:06:43.717	2024-03-12 23:06:43.717
380442	processingTriggers	processingTriggers-buildDependencyTrigger	\N	\N	buildDependencyTrigger	\N	MAIN_SERVER	MAIN_SERVER	58909494	\N	\N	2024-03-10 13:17:10.229321	2024-03-12 23:06:43.724	2024-03-12 23:06:43.724
380443	processingTriggers	processingTriggers-remoteRunOnBranch	\N	\N	remoteRunOnBranch	\N	MAIN_SERVER	MAIN_SERVER	55065896	\N	\N	2024-03-10 13:17:10.235952	2024-03-12 23:06:43.731	2024-03-12 23:06:43.731
380444	processingTriggers	processingTriggers-mavenArtifactDependencyTrigger	\N	\N	mavenArtifactDependencyTrigger	\N	MAIN_SERVER	MAIN_SERVER	52423615	\N	\N	2024-03-10 13:17:10.242945	2024-03-12 23:06:43.737	2024-03-12 23:06:43.737
380445	processingTriggers	processingTriggers-mavenSnapshotDependencyTrigger	\N	\N	mavenSnapshotDependencyTrigger	\N	MAIN_SERVER	MAIN_SERVER	52458169	\N	\N	2024-03-10 13:17:10.249647	2024-03-12 23:06:43.744	2024-03-12 23:06:43.744
380446	processingTriggers	processingTriggers-perforceShelveTrigger	\N	\N	perforceShelveTrigger	\N	MAIN_SERVER	MAIN_SERVER	58055088	\N	\N	2024-03-10 13:17:10.259225	2024-03-12 23:06:43.752	2024-03-12 23:06:43.752
380447	processingTriggers	processingTriggers-vcsTrigger	\N	\N	vcsTrigger	\N	MAIN_SERVER	MAIN_SERVER	61977558	\N	\N	2024-03-10 13:17:10.266194	2024-03-12 23:06:43.758	2024-03-12 23:06:43.758
380448	processingTriggers	processingTriggers-schedulingTrigger	\N	\N	schedulingTrigger	\N	MAIN_SERVER	MAIN_SERVER	50739333	\N	\N	2024-03-10 13:17:10.273748	2024-03-12 23:06:43.764	2024-03-12 23:06:43.764
380449	processingTriggers	processingTriggers-retryBuildTrigger	\N	\N	retryBuildTrigger	\N	MAIN_SERVER	MAIN_SERVER	61303184	\N	\N	2024-03-10 13:17:10.28114	2024-03-12 23:06:43.77	2024-03-12 23:06:43.77
388981	processingTriggers	processingTriggers-remoteRunOnBranch	\N	\N	remoteRunOnBranch	\N	MAIN_SERVER	MAIN_SERVER	1	\N	\N	2024-03-10 23:43:22.49329	\N	2024-03-12 23:16:34.71682
388982	processingTriggers	processingTriggers-mavenSnapshotDependencyTrigger	\N	\N	mavenSnapshotDependencyTrigger	\N	MAIN_SERVER	MAIN_SERVER	1	\N	\N	2024-03-10 23:43:22.499997	\N	2024-03-12 23:16:34.735413
388983	processingTriggers	processingTriggers-nuget.simple	\N	\N	nuget.simple	\N	MAIN_SERVER	MAIN_SERVER	1	\N	\N	2024-03-10 23:43:22.506492	\N	2024-03-12 23:16:34.746683
388984	processingTriggers	processingTriggers-perforceShelveTrigger	\N	\N	perforceShelveTrigger	\N	MAIN_SERVER	MAIN_SERVER	1	\N	\N	2024-03-10 23:43:22.512537	\N	2024-03-12 23:16:34.754248
388985	processingTriggers	processingTriggers-schedulingTrigger	\N	\N	schedulingTrigger	\N	MAIN_SERVER	MAIN_SERVER	1	\N	\N	2024-03-10 23:43:22.519088	\N	2024-03-12 23:16:34.761444
388986	processingTriggers	processingTriggers-retryBuildTrigger	\N	\N	retryBuildTrigger	\N	MAIN_SERVER	MAIN_SERVER	1	\N	\N	2024-03-10 23:43:22.528853	\N	2024-03-12 23:16:34.769646
388987	processingTriggers	processingTriggers-mavenArtifactDependencyTrigger	\N	\N	mavenArtifactDependencyTrigger	\N	MAIN_SERVER	MAIN_SERVER	1	\N	\N	2024-03-10 23:43:22.536036	\N	2024-03-12 23:16:34.776793
454453	processingTriggers	processingTriggers-schedulingTrigger	\N	\N	schedulingTrigger	\N	MAIN_SERVER	\N	0	\N	\N	2024-03-12 23:16:44.69192	\N	\N
454487	publishBuildStatus.buildQueued	publishBuildStatus.buildQueued:101	101	\N	TeamCity build was queued	\N	MAIN_SERVER	MAIN_SERVER	64560165	\N	\N	2024-03-12 23:17:19.925211	2024-03-12 23:17:19.944	2024-03-12 23:17:19.944
454488	publishBuildStatus.buildStarted	publishBuildStatus.buildStarted:101	101	\N	\N	\N	MAIN_SERVER	MAIN_SERVER	56550719	\N	\N	2024-03-12 23:17:20.167038	2024-03-12 23:17:20.196	2024-03-12 23:17:20.196
454617	publishBuildStatus.buildFinished	publishBuildStatus.buildFinished:101	101	\N	\N	\N	MAIN_SERVER	MAIN_SERVER	51801491	\N	\N	2024-03-12 23:19:43.043821	2024-03-12 23:19:43.062	2024-03-12 23:19:43.062
454616	finishBuild	finishBuild:101:0.1.2.cc625b80fa8d68a300397d498553649485c70862	101	\N	\N	\N	MAIN_SERVER	MAIN_SERVER	57857446	\N	\N	2024-03-12 23:19:42.647849	2024-03-12 23:19:43.192	2024-03-12 23:19:43.192
374681	processingTriggers	processingTriggers-buildDependencyTrigger	\N	\N	buildDependencyTrigger	\N	MAIN_SERVER	MAIN_SERVER	118483228	\N	\N	2024-03-10 06:16:49.843591	\N	\N
374682	processingTriggers	processingTriggers-mavenArtifactDependencyTrigger	\N	\N	mavenArtifactDependencyTrigger	\N	MAIN_SERVER	MAIN_SERVER	126801810	\N	\N	2024-03-10 06:16:49.850531	\N	\N
374683	processingTriggers	processingTriggers-nuget.simple	\N	\N	nuget.simple	\N	MAIN_SERVER	MAIN_SERVER	122474277	\N	\N	2024-03-10 06:16:49.856791	\N	\N
374684	processingTriggers	processingTriggers-remoteRunOnBranch	\N	\N	remoteRunOnBranch	\N	MAIN_SERVER	MAIN_SERVER	120580270	\N	\N	2024-03-10 06:16:49.8636	\N	\N
374685	processingTriggers	processingTriggers-perforceShelveTrigger	\N	\N	perforceShelveTrigger	\N	MAIN_SERVER	MAIN_SERVER	125085253	\N	\N	2024-03-10 06:16:49.870041	\N	\N
374686	processingTriggers	processingTriggers-schedulingTrigger	\N	\N	schedulingTrigger	\N	MAIN_SERVER	MAIN_SERVER	133545986	\N	\N	2024-03-10 06:16:49.878851	\N	\N
374687	processingTriggers	processingTriggers-retryBuildTrigger	\N	\N	retryBuildTrigger	\N	MAIN_SERVER	MAIN_SERVER	130044857	\N	\N	2024-03-10 06:16:49.885033	\N	\N
457877	publishBuildStatus.buildQueued	publishBuildStatus.buildQueued:102	102	\N	TeamCity build was queued	\N	MAIN_SERVER	MAIN_SERVER	66218638	\N	\N	2024-03-13 00:20:54.520202	2024-03-13 00:20:54.536	2024-03-13 00:20:54.536
457879	agentCommand:runBuild	agentId:1	\N	\N	\N	\N	MAIN_SERVER	\N	59267237	SUCCESS:<result><buildStartInfo><startTime>1710289254777</startTime><tz>Australia/Sydney</tz></buildStartInfo></result>	\N	2024-03-13 00:20:54.737434	2024-03-13 00:20:54.79	2024-03-13 00:20:54.79
388988	processingTriggers	processingTriggers-vcsTrigger	\N	\N	vcsTrigger	\N	MAIN_SERVER	MAIN_SERVER	1	\N	\N	2024-03-10 23:43:22.543726	\N	2024-03-12 23:16:34.784552
388989	processingTriggers	processingTriggers-buildDependencyTrigger	\N	\N	buildDependencyTrigger	\N	MAIN_SERVER	MAIN_SERVER	1	\N	\N	2024-03-10 23:43:22.551034	\N	2024-03-12 23:16:34.796941
454450	processingTriggers	processingTriggers-mavenArtifactDependencyTrigger	\N	\N	mavenArtifactDependencyTrigger	\N	MAIN_SERVER	\N	0	\N	\N	2024-03-12 23:16:44.652008	\N	\N
454451	processingTriggers	processingTriggers-retryBuildTrigger	\N	\N	retryBuildTrigger	\N	MAIN_SERVER	\N	0	\N	\N	2024-03-12 23:16:44.674118	\N	\N
454452	processingTriggers	processingTriggers-mavenSnapshotDependencyTrigger	\N	\N	mavenSnapshotDependencyTrigger	\N	MAIN_SERVER	\N	0	\N	\N	2024-03-12 23:16:44.680913	\N	\N
454454	processingTriggers	processingTriggers-buildDependencyTrigger	\N	\N	buildDependencyTrigger	\N	MAIN_SERVER	\N	0	\N	\N	2024-03-12 23:16:44.703467	\N	\N
457980	publishBuildStatus.buildFinished	publishBuildStatus.buildFinished:102	102	\N	\N	\N	MAIN_SERVER	MAIN_SERVER	53667665	\N	\N	2024-03-13 00:22:46.483861	2024-03-13 00:22:46.496	2024-03-13 00:22:46.496
457979	finishBuild	finishBuild:102:0.1.3.01be4c770e3295ab944eefdb9510511e7987b74e	102	\N	\N	\N	MAIN_SERVER	MAIN_SERVER	60934219	\N	\N	2024-03-13 00:22:46.226359	2024-03-13 00:22:46.543	2024-03-13 00:22:46.543
454455	processingTriggers	processingTriggers-vcsTrigger	\N	\N	vcsTrigger	\N	MAIN_SERVER	\N	0	\N	\N	2024-03-12 23:16:44.712384	\N	\N
454456	processingTriggers	processingTriggers-remoteRunOnBranch	\N	\N	remoteRunOnBranch	\N	MAIN_SERVER	\N	0	\N	\N	2024-03-12 23:16:44.722941	\N	\N
454457	processingTriggers	processingTriggers-nuget.simple	\N	\N	nuget.simple	\N	MAIN_SERVER	\N	0	\N	\N	2024-03-12 23:16:44.732626	\N	\N
454458	processingTriggers	processingTriggers-perforceShelveTrigger	\N	\N	perforceShelveTrigger	\N	MAIN_SERVER	\N	0	\N	\N	2024-03-12 23:16:44.742503	\N	\N
454486	buildChainChangesCollecting	101	101	\N	\N	\N	MAIN_SERVER	MAIN_SERVER	55369291	\N	\N	2024-03-12 23:17:16.943055	2024-03-12 23:17:19.933	2024-03-12 23:17:19.933
454489	agentCommand:runBuild	agentId:1	\N	\N	\N	\N	MAIN_SERVER	\N	63418528	SUCCESS:<result><buildStartInfo><startTime>1710285440677</startTime><tz>Australia/Sydney</tz></buildStartInfo></result>	\N	2024-03-12 23:17:20.504301	2024-03-12 23:17:20.711	2024-03-12 23:17:20.711
374688	processingTriggers	processingTriggers-mavenSnapshotDependencyTrigger	\N	\N	mavenSnapshotDependencyTrigger	\N	MAIN_SERVER	MAIN_SERVER	119644759	\N	\N	2024-03-10 06:16:49.891389	\N	\N
374689	processingTriggers	processingTriggers-vcsTrigger	\N	\N	vcsTrigger	\N	MAIN_SERVER	MAIN_SERVER	119054213	\N	\N	2024-03-10 06:16:49.898963	\N	\N
\.


--
-- TOC entry 6406 (class 0 OID 17453)
-- Dependencies: 368
-- Data for Name: node_tasks_long_value; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.node_tasks_long_value (uuid, long_value) FROM stdin;
\.


--
-- TOC entry 6313 (class 0 OID 16816)
-- Dependencies: 275
-- Data for Name: permanent_token_permissions; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.permanent_token_permissions (id, project_id, permission) FROM stdin;
\.


--
-- TOC entry 6312 (class 0 OID 16804)
-- Dependencies: 274
-- Data for Name: permanent_tokens; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.permanent_tokens (id, identifier, name, user_id, hashed_value, expiration_time, creation_time, last_access_time, last_access_info) FROM stdin;
\.


--
-- TOC entry 6379 (class 0 OID 17282)
-- Dependencies: 341
-- Data for Name: personal_build_relative_path; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.personal_build_relative_path (build_id, original_path_hash, relative_path) FROM stdin;
\.


--
-- TOC entry 6327 (class 0 OID 16916)
-- Dependencies: 289
-- Data for Name: personal_vcs_change; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.personal_vcs_change (modification_id, file_num, vcs_file_name, vcs_file_name_hash, relative_file_name_pos, relative_file_name, relative_file_name_hash, change_type, change_name, before_revision, after_revision) FROM stdin;
\.


--
-- TOC entry 6414 (class 0 OID 17508)
-- Dependencies: 376
-- Data for Name: personal_vcs_changes; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.personal_vcs_changes (modification_id, change_name, change_type, before_revision, after_revision, vcs_file_name, relative_file_name) FROM stdin;
\.


--
-- TOC entry 6326 (class 0 OID 16904)
-- Dependencies: 288
-- Data for Name: personal_vcs_history; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.personal_vcs_history (modification_id, modification_hash, user_id, description, change_date, changes_count, commit_changes, status, scheduled_for_deletion) FROM stdin;
\.


--
-- TOC entry 6317 (class 0 OID 16844)
-- Dependencies: 279
-- Data for Name: problem; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.problem (problem_id, problem_type, problem_identity) FROM stdin;
\.


--
-- TOC entry 6278 (class 0 OID 16581)
-- Dependencies: 240
-- Data for Name: project; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.project (int_id, config_id, origin_project_id, delete_time) FROM stdin;
_Root	a177d188-0481-441b-94c6-32f51863ef64	\N	\N
project1	a989bd24-f6d0-4c46-a174-2b0e867f2111	\N	\N
\.


--
-- TOC entry 6392 (class 0 OID 17369)
-- Dependencies: 354
-- Data for Name: project_files; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.project_files (file_id, file_name) FROM stdin;
\.


--
-- TOC entry 6281 (class 0 OID 16602)
-- Dependencies: 243
-- Data for Name: project_mapping; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.project_mapping (int_id, ext_id, main) FROM stdin;
_Root	_Root	1
project1	Fortitude	1
\.


--
-- TOC entry 6311 (class 0 OID 16799)
-- Dependencies: 273
-- Data for Name: remember_me; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.remember_me (user_key, secure) FROM stdin;
2019384512^1	6739618876871694729
\.


--
-- TOC entry 6335 (class 0 OID 16990)
-- Dependencies: 297
-- Data for Name: removed_builds_history; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.removed_builds_history (build_id, agent_name, build_type_id, build_start_time_server, build_start_time_agent, build_finish_time_server, status, status_text, user_status_text, pin, is_personal, is_canceled, build_number, requestor, queued_time, remove_from_queue_time, build_state_id, agent_type_id, branch_name) FROM stdin;
\.


--
-- TOC entry 6380 (class 0 OID 17289)
-- Dependencies: 342
-- Data for Name: responsibilities; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.responsibilities (problem_id, state, responsible_user_id, reporter_user_id, timestmp, remove_method, comments) FROM stdin;
\.


--
-- TOC entry 6333 (class 0 OID 16966)
-- Dependencies: 295
-- Data for Name: running; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.running (build_id, agent_id, build_type_id, build_start_time_agent, build_start_time_server, build_finish_time_server, last_build_activity_time, is_personal, build_number, build_counter, requestor, access_code, queued_ag_restr_type_id, queued_ag_restr_id, build_state_id, agent_type_id, user_status_text, progress_text, current_path_text) FROM stdin;
\.


--
-- TOC entry 6416 (class 0 OID 17525)
-- Dependencies: 378
-- Data for Name: server; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.server (server_id) FROM stdin;
1706960303447
\.


--
-- TOC entry 6410 (class 0 OID 17483)
-- Dependencies: 372
-- Data for Name: server_health_items; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.server_health_items (id, report_id, category_id, item_id) FROM stdin;
1	NotConfiguredRootUrlReport	not_configured_root_url	not_configured_root_url
2	NewVersionAvailable	new_tc_version	147586
\.


--
-- TOC entry 6272 (class 0 OID 16546)
-- Dependencies: 234
-- Data for Name: server_property; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.server_property (prop_name, prop_value) FROM stdin;
***	8f80140d1868578ad3b346d06d25f6c10000
LICENSE_AGREEMENT	true
\.


--
-- TOC entry 6403 (class 0 OID 17433)
-- Dependencies: 365
-- Data for Name: server_statistics; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.server_statistics (metric_key, metric_value, metric_timestamp) FROM stdin;
1	0	1706960399100
3	3	1706960399100
2	0	1706960399100
1	0	1706963999150
3	3	1706963999150
2	0	1706963999150
1	0	1706996481094
3	3	1706996481094
2	0	1706996481094
1	0	1707000081005
3	3	1707000081005
2	0	1707000081005
1	0	1707004784076
3	3	1707004784076
2	0	1707004784076
1	0	1707023650165
3	3	1707023650165
2	0	1707023650165
1	0	1707030318252
3	3	1707030318252
2	0	1707030318252
1	0	1707076363209
3	3	1707076363209
2	0	1707076363209
1	0	1707113996053
3	3	1707113996053
2	0	1707113996053
1	0	1707117596049
3	3	1707117596049
2	0	1707117596049
1	0	1707125096925
3	3	1707125096925
2	0	1707125096925
1	1	1707128696873
3	3	1707128696873
2	1	1707128696873
1	1	1707131726092
3	3	1707131726092
2	0	1707131726092
1	1	1708827374230
3	3	1708827374230
2	0	1708827374230
1	1	1708827680469
3	3	1708827680469
2	0	1708827680469
1	1	1708831280458
3	3	1708831280458
2	0	1708831280458
1	1	1708843612836
3	3	1708843612836
2	0	1708843612836
1	1	1708847212833
3	3	1708847212833
2	0	1708847212833
1	1	1708850812819
3	3	1708850812819
2	0	1708850812819
1	1	1708854412813
3	3	1708854412813
2	0	1708854412813
1	1	1708858012806
3	3	1708858012806
2	0	1708858012806
1	1	1708895168508
3	3	1708895168508
2	0	1708895168508
1	1	1708977503797
3	3	1708977503797
2	0	1708977503797
1	1	1709060714111
3	3	1709060714111
2	0	1709060714111
1	1	1709064314102
3	3	1709064314102
2	0	1709064314102
1	1	1709151121736
3	3	1709151121736
2	0	1709151121736
1	1	1709185629518
3	3	1709185629518
2	0	1709185629518
1	1	1709189229537
3	3	1709189229537
2	0	1709189229537
1	1	1709197259030
3	3	1709197259030
2	0	1709197259030
1	1	1709200859024
3	3	1709200859024
2	0	1709200859024
1	1	1709237236881
3	3	1709237236881
2	0	1709237236881
1	1	1709240836828
3	3	1709240836828
2	0	1709240836828
1	1	1709244436794
3	3	1709244436794
2	0	1709244436794
1	1	1709248036776
3	3	1709248036776
2	0	1709248036776
1	1	1709251636759
3	3	1709251636759
2	0	1709251636759
1	1	1709255236749
3	3	1709255236749
2	0	1709255236749
1	1	1709258836741
3	3	1709258836741
2	0	1709258836741
1	1	1709262436735
3	3	1709262436735
2	0	1709262436735
1	1	1709266036720
3	3	1709266036720
2	0	1709266036720
1	1	1709269638115
3	3	1709269638115
2	0	1709269638115
1	1	1709273238108
3	3	1709273238108
2	0	1709273238108
1	1	1709276838100
3	3	1709276838100
2	0	1709276838100
1	1	1709280438092
3	3	1709280438092
2	0	1709280438092
1	1	1709284038079
3	3	1709284038079
2	0	1709284038079
1	1	1709287638047
3	3	1709287638047
2	0	1709287638047
1	1	1709291238031
3	3	1709291238031
2	0	1709291238031
1	1	1709339696403
3	3	1709339696403
2	0	1709339696403
1	1	1709343296392
3	3	1709343296392
2	0	1709343296392
1	1	1709346896387
3	3	1709346896387
2	0	1709346896387
1	1	1709350496376
3	3	1709350496376
2	0	1709350496376
1	1	1709362331225
3	3	1709362331225
2	0	1709362331225
1	1	1709365931204
3	3	1709365931204
2	0	1709365931204
1	1	1709369531195
3	3	1709369531195
2	0	1709369531195
1	1	1709419252804
3	3	1709419252804
2	0	1709419252804
1	1	1709431492982
3	3	1709431492982
2	0	1709431492982
1	1	1709435092962
3	3	1709435092962
2	0	1709435092962
1	1	1709438692952
3	3	1709438692952
2	0	1709438692952
1	1	1709442292930
3	3	1709442292930
2	0	1709442292930
1	1	1709445892918
3	3	1709445892918
2	0	1709445892918
1	1	1709449492898
3	3	1709449492898
2	0	1709449492898
1	1	1709453092884
3	3	1709453092884
2	0	1709453092884
1	1	1709456692852
3	3	1709456692852
2	0	1709456692852
1	1	1709460292795
3	3	1709460292795
2	0	1709460292795
1	1	1709463892765
3	3	1709463892765
2	0	1709463892765
1	1	1709467492744
3	3	1709467492744
2	0	1709467492744
1	1	1709471092729
3	3	1709471092729
2	0	1709471092729
1	1	1709497264097
3	3	1709497264097
2	0	1709497264097
1	1	1709623230209
3	3	1709623230209
2	0	1709623230209
1	1	1709626830194
3	3	1709626830194
2	0	1709626830194
1	1	1709630430180
3	3	1709630430180
2	0	1709630430180
1	1	1709634030169
3	3	1709634030169
2	0	1709634030169
1	1	1709637630156
3	3	1709637630156
2	0	1709637630156
1	1	1709641230141
3	3	1709641230141
2	0	1709641230141
1	1	1709644830129
3	3	1709644830129
2	0	1709644830129
1	1	1709668303932
3	3	1709668303932
2	0	1709668303932
1	1	1709671903915
3	3	1709671903915
2	0	1709671903915
1	1	1709709588027
3	3	1709709588027
2	0	1709709588027
1	1	1709716934280
3	3	1709716934280
2	0	1709716934280
1	1	1709720534265
3	3	1709720534265
2	0	1709720534265
1	1	1709752010198
3	3	1709752010198
2	0	1709752010198
1	1	1709755610188
3	3	1709755610188
2	0	1709755610188
1	1	1709759210176
3	3	1709759210176
2	0	1709759210176
1	1	1709763155128
3	3	1709763155128
2	0	1709763155128
1	1	1709766755122
3	3	1709766755122
2	0	1709766755122
1	1	1709770355109
3	3	1709770355109
2	0	1709770355109
1	1	1709773955101
3	3	1709773955101
2	0	1709773955101
1	1	1709777555087
3	3	1709777555087
2	0	1709777555087
1	1	1709781155077
3	3	1709781155077
2	0	1709781155077
1	1	1709784755064
3	3	1709784755064
2	0	1709784755064
1	1	1709788355023
3	3	1709788355023
2	0	1709788355023
1	1	1709791955002
3	3	1709791955002
2	0	1709791955002
1	1	1709795554991
3	3	1709795554991
2	0	1709795554991
1	1	1709799684898
3	3	1709799684898
2	0	1709799684898
1	1	1709803284876
3	3	1709803284876
2	0	1709803284876
1	1	1709806884860
3	3	1709806884860
2	0	1709806884860
1	1	1709810484851
3	3	1709810484851
2	0	1709810484851
1	1	1709844558452
3	3	1709844558452
2	0	1709844558452
1	1	1709854111893
3	3	1709854111893
2	0	1709854111893
1	1	1709890707101
3	3	1709890707101
2	0	1709890707101
1	1	1709894470825
3	3	1709894470825
2	0	1709894470825
1	1	1709898070815
3	3	1709898070815
2	0	1709898070815
1	1	1709933990550
3	3	1709933990550
2	0	1709933990550
1	1	1709937590540
3	3	1709937590540
2	0	1709937590540
1	1	1709941190525
3	3	1709941190525
2	0	1709941190525
1	1	1709949874036
3	3	1709949874036
2	0	1709949874036
1	1	1709965395002
3	3	1709965395002
2	0	1709965395002
1	1	1709968993945
3	3	1709968993945
2	0	1709968993945
1	1	1709972593923
3	3	1709972593923
2	0	1709972593923
1	1	1709976193900
3	3	1709976193900
2	0	1709976193900
1	1	1709979793882
3	3	1709979793882
2	0	1709979793882
1	1	1709983393868
3	3	1709983393868
2	0	1709983393868
1	1	1710022089781
3	3	1710022089781
2	0	1710022089781
1	1	1710025689758
3	3	1710025689758
2	0	1710025689758
1	1	1710029289747
3	3	1710029289747
2	0	1710029289747
1	1	1710032889733
3	3	1710032889733
2	0	1710032889733
1	1	1710036489719
3	3	1710036489719
2	0	1710036489719
1	1	1710040089705
3	3	1710040089705
2	0	1710040089705
1	1	1710043689690
3	3	1710043689690
2	0	1710043689690
1	1	1710047289675
3	3	1710047289675
2	0	1710047289675
1	1	1710050889666
3	3	1710050889666
2	0	1710050889666
1	1	1710051470037
3	3	1710051470037
2	0	1710051470037
1	1	1710055071342
3	3	1710055071342
2	0	1710055071342
1	1	1710075873542
3	3	1710075873542
2	0	1710075873542
1	1	1710076276454
3	3	1710076276454
2	0	1710076276454
1	1	1710076690432
3	3	1710076690432
2	0	1710076690432
1	1	1710104886656
3	3	1710104886656
2	0	1710104886656
1	1	1710111792701
3	3	1710111792701
2	0	1710111792701
1	1	1710114262677
3	3	1710114262677
2	0	1710114262677
1	1	1710117862660
3	3	1710117862660
2	0	1710117862660
1	1	1710121462649
3	3	1710121462649
2	0	1710121462649
1	1	1710125062637
3	3	1710125062637
2	0	1710125062637
1	1	1710128662622
3	3	1710128662622
2	0	1710128662622
1	1	1710132262612
3	3	1710132262612
2	0	1710132262612
1	1	1710135863654
3	3	1710135863654
2	0	1710135863654
1	1	1710143085156
3	3	1710143085156
2	0	1710143085156
1	1	1710146685137
3	3	1710146685137
2	0	1710146685137
1	1	1710150285125
3	3	1710150285125
2	0	1710150285125
1	1	1710153885110
3	3	1710153885110
2	0	1710153885110
1	1	1710186472045
3	3	1710186472045
2	0	1710186472045
1	1	1710190072032
3	3	1710190072032
2	0	1710190072032
1	1	1710196628369
3	3	1710196628369
2	0	1710196628369
1	1	1710230659020
3	3	1710230659020
2	0	1710230659020
1	1	1710234259012
3	3	1710234259012
2	0	1710234259012
1	1	1710237859004
3	3	1710237859004
2	0	1710237859004
1	1	1710241979415
3	3	1710241979415
2	0	1710241979415
1	1	1710245579396
3	3	1710245579396
2	0	1710245579396
1	1	1710275035705
3	3	1710275035705
2	0	1710275035705
1	1	1710283115441
3	3	1710283115441
2	0	1710283115441
1	1	1710285454612
3	3	1710285454612
2	1	1710285454612
1	1	1710289054601
3	3	1710289054601
2	1	1710289054601
\.


--
-- TOC entry 6271 (class 0 OID 16543)
-- Dependencies: 233
-- Data for Name: single_row; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.single_row (dummy_field) FROM stdin;
X
\.


--
-- TOC entry 6370 (class 0 OID 17223)
-- Dependencies: 332
-- Data for Name: stats; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.stats (build_id, test_count) FROM stdin;
1	1369
101	1369
102	1369
\.


--
-- TOC entry 6397 (class 0 OID 17398)
-- Dependencies: 359
-- Data for Name: stats_publisher_state; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.stats_publisher_state (metric_id, value) FROM stdin;
-5264660594081243009	0
-4539345364261023841	0
8170543957031342021	0
6662056484837638591	0
-3896958621569208557	0
9018439616396414058	0
-1749670243861784615	0
-6826520637656720981	0
-6371738409877395985	0
-959614572394077983	0
3014510524022395756	0
5744866293795496522	0
-6912521911911837242	0
-6022911269954286022	0
8605895206854366408	0
\.


--
-- TOC entry 6367 (class 0 OID 17204)
-- Dependencies: 329
-- Data for Name: test_failure_rate; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.test_failure_rate (build_type_id, test_name_id, success_count, failure_count, last_failure_time) FROM stdin;
\.


--
-- TOC entry 6342 (class 0 OID 17039)
-- Dependencies: 304
-- Data for Name: test_info; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.test_info (build_id, test_id, test_name_id, status, duration) FROM stdin;
1	2000000000	-5168067613992917628	1	121
1	2000000001	1658048510990355987	1	512
1	2000000002	395155716920624105	1	17
1	2000000003	2105868547303546195	1	1
1	2000000004	4168344068604504176	1	25
1	2000000005	-599379416200460755	1	5
1	2000000006	5865893109958142336	1	16
1	2000000007	-1014448920728114714	1	9
1	2000000008	-5914716010989158113	1	4
1	2000000009	4226214848212862725	1	3
1	2000000010	4142698781433296355	1	4
1	2000000011	891115598539416633	1	0
1	2000000012	-8247447631318334064	1	3
1	2000000013	5039604955451317509	1	1
1	2000000014	320849263338300981	1	1
1	2000000015	-5488696493139541078	1	0
1	2000000016	1097231189897963555	1	0
1	2000000017	-7740681692428656038	1	2
1	2000000018	6553368331669324775	1	0
1	2000000019	-8986029802410366237	1	1
1	2000000020	1605842430492936233	1	0
1	2000000021	104097758823551538	1	2
1	2000000022	5914280589009734834	1	3
1	2000000023	4213147678250946958	1	2
1	2000000024	6183954501267872216	1	1
1	2000000025	-5205656477440234433	1	1
1	2000000026	-7493652208583830495	1	3
1	2000000027	2908739203109759186	1	0
1	2000000028	5130518625500238005	1	0
1	2000000029	7882878838845453136	1	0
1	2000000030	6892766825010809023	1	0
1	2000000031	-5663271173113375380	1	0
1	2000000032	9154034748061152465	1	0
1	2000000033	-1357977716921926776	1	1
1	2000000034	-6629809875920628607	1	0
1	2000000035	-5312011533091652985	1	2
1	2000000036	718348107055543106	1	10
1	2000000037	-1157373951113888645	1	1
1	2000000038	3837923844096457385	1	9
1	2000000039	-7864049997018424615	1	0
1	2000000040	-2628665152080434732	1	0
1	2000000041	7784828775858321386	1	0
1	2000000042	8687322539772689954	1	0
1	2000000043	-8965823899831416089	1	0
1	2000000044	-5014417892241219914	1	0
1	2000000045	5317262931568349904	1	7
1	2000000046	982545823736371990	1	11
1	2000000047	-6375807517106078134	1	1
1	2000000048	3884006348627733517	1	2
1	2000000049	3773868157680640943	1	0
1	2000000050	4133740822589071545	1	4
1	2000000051	-631540076145704700	1	3
1	2000000052	471873135801994195	1	158
1	2000000053	-8211194052941157511	1	0
1	2000000054	4869087816672658090	1	11
1	2000000055	7307247278838092431	1	9
1	2000000056	-6754945327394567629	1	1
1	2000000057	-7530077675173574284	1	3
1	2000000058	-6813819657005622371	1	0
1	2000000059	5013891203746348224	1	0
1	2000000060	8139150691954259558	1	0
1	2000000061	-6863013941068350251	1	0
1	2000000062	8099482474164728756	1	3
1	2000000063	3502479247353265537	1	6
1	2000000064	4421890388133832713	1	1
1	2000000065	-1225547086238709623	1	1
1	2000000066	6004521876572272844	1	0
1	2000000067	-8579785066513323788	1	2
1	2000000068	5084515830392146249	1	5
1	2000000069	-7631616222065932609	1	25
1	2000000070	4054285518414936991	1	0
1	2000000071	855622501930378268	1	4
1	2000000072	4822464996449682968	1	1
1	2000000073	4804759450788955081	1	0
1	2000000074	2615522436833709908	1	0
1	2000000075	-5893936263433988855	1	0
1	2000000076	-2806122811888598155	1	0
1	2000000077	-323908055081131550	1	0
1	2000000078	-4228791055132281238	1	0
1	2000000079	4585975189901988855	1	0
1	2000000080	-1562883777249695314	1	0
1	2000000081	-2261324999257812422	1	0
1	2000000082	385756262405517557	1	0
1	2000000083	-1976503402141874080	1	0
1	2000000084	4525651972397617056	1	0
1	2000000085	828119402709425352	1	0
1	2000000086	5598680601370150278	1	0
1	2000000087	-3290616611505052031	1	20
1	2000000088	-3075720102990604265	1	0
1	2000000089	-4205356829618576577	1	0
1	2000000090	1541999056923217352	1	0
1	2000000091	-6729386337864249509	1	0
1	2000000092	-1636241169476163454	1	2
1	2000000093	-842028361149352176	1	0
1	2000000094	-5001296628537403263	1	0
1	2000000095	-4895682748520660873	1	316
1	2000000096	9207039181192489174	1	5
1	2000000097	265299714377165253	1	2
1	2000000098	1850931895478759887	1	38
1	2000000099	-3858185599271528368	1	11
1	2000000100	5369156447176937088	1	4
1	2000000101	2682672937639856679	1	2
1	2000000102	7644799529120265731	1	8
1	2000000103	546148726491252312	1	3
1	2000000104	-6925810533630939515	1	2
1	2000000105	2801260725862711702	1	2
1	2000000106	3985171750648239103	1	1
1	2000000107	-2094885574236634978	1	1
1	2000000108	-7293008259388994363	1	8
1	2000000109	8289558281556610212	1	22
1	2000000110	-8359869987379225846	1	9
1	2000000111	4643082334228461166	1	6
1	2000000112	-7182649009264205024	1	6
1	2000000113	-262372815853356321	1	6
1	2000000114	2853305180478063174	1	5
1	2000000115	273740604063040256	1	6
1	2000000116	5910254892637388891	1	64
1	2000000117	7296555989703615511	1	5
1	2000000118	-1647803935715495851	1	13
1	2000000119	-7216378367928879894	1	5
1	2000000120	2642972094844077548	1	5
1	2000000121	6061745905256248282	1	7
1	2000000122	4178047494179091499	1	17
1	2000000123	7899792671160188869	1	12
1	2000000124	-7946802209278770662	1	7
1	2000000125	-2095179500710035246	1	7
1	2000000126	-5370593418204654093	1	5
1	2000000127	5970637498973245149	1	6
1	2000000128	-5696787878964117539	1	6
1	2000000129	3886766662433091017	1	10
1	2000000130	8292494691086061859	1	8
1	2000000131	4985031533047976606	1	6
1	2000000132	8549850915877672668	1	9
1	2000000133	8229997000000531653	1	7
1	2000000134	-9128183645133246639	1	6
1	2000000135	7218161957043746000	1	6
1	2000000136	-7197118442584039090	1	12
1	2000000137	2498548561942185946	1	6
1	2000000138	8286629246610762820	1	7
1	2000000139	-1174808756205235933	1	5
1	2000000140	-8304354904086372845	1	5
1	2000000141	-901036977350955796	1	12
1	2000000142	6165186261284916642	1	37
1	2000000143	5961349759012781409	1	170
1	2000000144	3358260765246435230	1	6
1	2000000145	3541929903117895034	1	5
1	2000000146	-1084510063449187158	1	8
1	2000000147	3457924317783329288	1	7
1	2000000148	8805228457669272158	1	2
1	2000000149	5062942869313641847	1	4
1	2000000150	8881830009048021057	1	26
1	2000000151	6762983883462077005	1	3
1	2000000152	-5747231119492549359	1	3
1	2000000153	3222081366833223268	1	8
1	2000000154	2466633683436661857	1	1
1	2000000155	-1575323095389685462	1	4
1	2000000156	-802959555982311160	1	2
1	2000000157	-2304382874982447364	1	15
1	2000000158	-7215277425977975246	1	1
1	2000000159	-1094041455508475148	1	0
1	2000000160	3421573324132374923	1	1
1	2000000161	-9207514556493929158	1	0
1	2000000162	4504696974646285517	1	0
1	2000000163	5780058753660829978	1	0
1	2000000164	5788044238413357566	1	6
1	2000000165	45366133087923521	1	2
1	2000000166	-3107720997913478708	1	0
1	2000000167	-7414640722507565204	1	28
1	2000000168	2728663142375709036	1	1
1	2000000169	5532191644254049702	1	0
1	2000000170	-2575506855031989672	1	0
1	2000000171	3913174321725019164	1	0
1	2000000172	-6325375061704290982	1	2
1	2000000173	1952829678854231830	1	0
1	2000000174	-7714358181202295009	1	0
1	2000000175	167381447430374963	1	67
1	2000000176	-7156862266043976418	1	30
1	2000000177	7453602060532121344	1	37
1	2000000178	2048672468661446044	1	0
1	2000000179	-5313316152668899945	1	0
1	2000000180	6546417840909422353	1	0
1	2000000181	7998291862378205807	1	0
1	2000000182	1583629675819367816	1	16
1	2000000183	6015330456467304167	1	1
1	2000000184	-3922533019978291516	1	1
1	2000000185	6469926083620759826	1	0
1	2000000186	-6197704211498601688	1	15
1	2000000187	7506018132291755046	1	7
1	2000000188	-2854098181133654390	1	1
1	2000000189	5580666055292867697	1	0
1	2000000190	-5586930227402413306	1	1
1	2000000191	-5863135281082852428	1	1
1	2000000192	6876094986674014987	1	10
1	2000000193	-4387068365724119821	1	0
1	2000000194	7685242590091901238	1	5
1	2000000195	-2846826406673728752	1	33
1	2000000196	9174150359974885957	1	17
1	2000000197	-2872144303876561253	1	21
1	2000000198	-3668471866226188888	1	50
1	2000000199	2286211212203948379	1	37
1	2000000200	3876868695864254709	1	31
1	2000000201	-8982279052801127019	1	22
1	2000000202	-1191290804749559924	1	18
1	2000000203	3188356211687288195	1	26
1	2000000204	8158340901452721225	1	120
1	2000000205	6159620885790173160	1	138
1	2000000206	3132744306913834556	1	13
1	2000000207	-3207103653603210855	1	9
1	2000000208	-8222704019872743726	1	7
1	2000000209	-8768095674480220028	1	0
1	2000000210	4768003828651437716	1	0
1	2000000211	8906314032266130370	1	2
1	2000000212	-7100702980283025836	1	1
1	2000000213	1344233066732476800	1	1
1	2000000214	7863800826459049259	1	1
1	2000000215	-5635970143258691064	1	0
1	2000000216	399603175506464863	1	1
1	2000000217	-4159700018826741568	1	0
1	2000000218	-1217956621720234165	1	0
1	2000000219	-4379048972556788231	1	3
1	2000000220	6910590723172147604	1	1
1	2000000221	-5436223351934266950	1	0
1	2000000222	3385906740431711903	1	0
1	2000000223	1015688014506287140	1	0
1	2000000224	-4545065987677879872	1	0
1	2000000225	-1690653735689708521	1	1
1	2000000226	9152719559837512697	1	1
1	2000000227	2112934594205575010	1	0
1	2000000228	-6921788027242763511	1	0
1	2000000229	9026668089164890926	1	0
1	2000000230	7835985019669643007	1	0
1	2000000231	1748930001085728374	1	0
1	2000000232	-4870757489143658629	1	10
1	2000000233	-8946682362426085462	1	0
1	2000000234	6482940291938471003	1	0
1	2000000235	-5100029622945035865	1	1
1	2000000236	5861494255440031174	1	1
1	2000000237	-3704034302743523421	1	0
1	2000000238	-3910353358171428391	1	0
1	2000000239	7178471542052155939	1	0
1	2000000240	-4846440185093465276	1	0
1	2000000241	-5098932985785461169	1	0
1	2000000242	3253163512775453786	1	0
1	2000000243	-2790709064176103988	1	1
1	2000000244	871906643331371361	1	0
1	2000000245	-4909190956136147169	1	0
1	2000000246	-811238283709010921	1	0
1	2000000247	-3044470657175993418	1	4
1	2000000248	7882824793751932003	1	0
1	2000000249	7742639209789501280	1	0
1	2000000250	-2046409902734848786	1	0
1	2000000251	-5682229402934137803	1	0
1	2000000252	-6243293462249496788	1	1
1	2000000253	8170965163082980576	1	0
1	2000000254	-1626235145649623620	1	0
1	2000000255	854370132355022024	1	0
1	2000000256	3273353310598347926	1	0
1	2000000257	8901378153430543944	1	1
1	2000000258	-6177029893441046382	1	0
1	2000000259	-7480954548352835387	1	0
1	2000000260	-3716360097816402166	1	0
1	2000000261	-1514631603694905049	1	0
1	2000000262	5140311844596731374	1	0
1	2000000263	671267312631720891	1	0
1	2000000264	-8631849075709006578	1	1
1	2000000265	3441965872625483974	1	0
1	2000000266	-5685808563646212420	1	0
1	2000000267	-5859128146916308836	1	0
1	2000000268	42973866600002177	1	1
1	2000000269	3442892229187295721	1	0
1	2000000270	-8635253897124206828	1	0
1	2000000271	5460041067622205790	1	0
1	2000000272	8068526363439912750	1	0
1	2000000273	-3276954727298532722	1	0
1	2000000274	-3844892461784491471	1	0
1	2000000275	8984070560824590680	1	0
1	2000000276	-6791269373554175731	1	1
1	2000000277	5413119706348108665	1	0
1	2000000278	-5649409396766215112	1	0
1	2000000279	-2956507839275432988	1	0
1	2000000280	6304524793301695953	1	0
1	2000000281	-2531294556859143024	1	0
1	2000000282	-3591361680031440861	1	1
1	2000000283	-4726818166408084722	1	0
1	2000000284	9160233208864691179	1	2
1	2000000285	8439687064434765458	1	0
1	2000000286	333483086096992788	1	0
1	2000000287	-2294112922519809294	1	1
1	2000000288	-1782386555252545714	1	0
1	2000000289	-2319932335302408233	1	1
1	2000000290	-6941344528592049593	1	1
1	2000000291	-8387929219434347608	1	0
1	2000000292	5534209550501235227	1	0
1	2000000293	-7842144647537605462	1	0
1	2000000294	3529563026656807306	1	0
1	2000000295	3292015599141719441	1	0
1	2000000296	-1956624619424199379	1	0
1	2000000297	2216113875507654384	1	0
1	2000000298	-6962178826515084706	1	27
1	2000000299	8840354425581028533	1	0
1	2000000300	-4659232289303006199	1	0
1	2000000301	2893223015212878224	1	11
1	2000000302	500549125022718037	1	9
1	2000000303	1596753244653843244	1	8
1	2000000304	2317779630936083971	1	6
1	2000000305	-4790867754608461845	1	5
1	2000000306	8355975946991213234	1	5
1	2000000307	4525469924466266750	1	55
1	2000000308	8339961779989768797	1	389
1	2000000309	6768425885042804800	1	375
1	2000000310	-3702124768431439213	1	3
1	2000000311	-2151332099485546499	1	1
1	2000000312	6805988862252946054	1	1
1	2000000313	4186957571204407323	1	1
1	2000000314	-1137552624966346092	1	0
1	2000000315	-6843401644120111781	1	1
1	2000000316	-4775450474354578645	1	0
1	2000000317	673073708176228709	1	1
1	2000000318	-6042258967121502890	1	1
1	2000000319	5454844165586025251	1	1
1	2000000320	4781022324913180018	1	156
1	2000000321	7198195242312020334	1	1
1	2000000322	8061551208747540058	1	0
1	2000000323	975291638248038318	1	1
1	2000000324	5296186274952516017	1	0
1	2000000325	-7220900720455481494	1	0
1	2000000326	4598769653297822368	1	0
1	2000000327	5557147014809399260	1	1
1	2000000328	-6300936569192378386	1	0
1	2000000329	6583713035894075762	1	0
1	2000000330	-7148742707063303148	1	5
1	2000000331	6891133056092466157	1	5
1	2000000332	8847504368228341853	1	3
1	2000000333	-769850871404066714	1	3
1	2000000334	8010487579726324572	1	3
1	2000000335	-1281815806852441397	1	3
1	2000000336	1938521020609095059	1	0
1	2000000337	3523884679671725957	1	1
1	2000000338	810398108306760348	1	8
1	2000000339	-3753456124963804734	1	6
1	2000000340	9055249480909265145	1	0
1	2000000341	1376848864724890411	1	1
1	2000000342	-7249976424530137073	1	1
1	2000000343	4964813406853494573	1	1
1	2000000344	-4591010665574936763	1	2
1	2000000345	287894396417678741	1	3
1	2000000346	-3260839014246629641	1	126
1	2000000347	4089557322633378077	1	4
1	2000000348	-37434540835908810	1	1
1	2000000349	-6012374541873459228	1	1
1	2000000350	-8900759627960524397	1	0
1	2000000351	-6743800908559635352	1	0
1	2000000352	8199428507212141948	1	0
1	2000000353	-7201485080435899140	1	0
1	2000000354	1921674646476416089	1	0
1	2000000355	-1963231258293348397	1	0
1	2000000356	-8218325220138309565	1	0
1	2000000357	-5848613183465042385	1	0
1	2000000358	7878477018103303313	1	0
1	2000000359	-652171486573031821	1	0
1	2000000360	-6985289698676835606	1	0
1	2000000361	-5041753597538503662	1	0
1	2000000362	3530066322638971749	1	0
1	2000000363	-2370561505487284775	1	0
1	2000000364	1672842826120172223	1	0
1	2000000365	-2389231846021142751	1	0
1	2000000366	-5707071873651388052	1	0
1	2000000367	5504638158992425192	1	0
1	2000000368	-3103166860339059077	1	0
1	2000000369	-8762091848778466957	1	0
1	2000000370	-2138427480696216105	1	0
1	2000000371	-8727409099088109660	1	0
1	2000000372	-5455537310022155887	1	1
1	2000000373	-167802844271584258	1	0
1	2000000374	2198556615269313949	1	0
1	2000000375	7040673462681810840	1	0
1	2000000376	123339232929051273	1	0
1	2000000377	-3971936715945561297	1	0
1	2000000378	-5889541757896086778	1	0
1	2000000379	7380966007935825479	1	0
1	2000000380	5345430275619616017	1	0
1	2000000381	-3268371542770224896	1	0
1	2000000382	-9150411862071801368	1	0
1	2000000383	-8627233018830102918	1	0
1	2000000384	2383175652925770064	1	0
1	2000000385	-3529455259661086276	1	0
1	2000000386	8112924697463551049	1	0
1	2000000387	-3889480022973573131	1	10
1	2000000388	-554800153383183542	1	42
1	2000000389	3174605870365863445	1	8
1	2000000390	-6010461173274098506	1	4
1	2000000391	3972174041477632771	1	8
1	2000000392	-599618953858237035	1	1
1	2000000393	-3332655978695007023	1	4
1	2000000394	3334900419860551377	1	2
1	2000000395	-8391786289264016656	1	1
1	2000000396	2324740851889752724	1	15
1	2000000397	-1306535233403231905	1	4
1	2000000398	3691910877796480479	1	3
1	2000000399	4101574638442679856	1	4
1	2000000400	2528460505267493704	1	1
1	2000000401	3868823698102365907	1	26
1	2000000402	1210847126746082376	1	1
1	2000000403	6274690045188883515	1	2
1	2000000404	2448275589862489061	1	2
1	2000000405	1019708284290908756	1	1
1	2000000406	5731224940909260024	1	3
1	2000000407	-4461578282072854891	1	0
1	2000000408	4422570313003487721	1	1
1	2000000409	-6975057738140130921	1	0
1	2000000410	2583041290795623063	1	1
1	2000000411	4554372258448489687	1	0
1	2000000412	4676693325818911216	1	0
1	2000000413	-6899431719471549278	1	4
1	2000000414	3509701785538232647	1	1
1	2000000415	-9072473139632003956	1	4
1	2000000416	-8218772127320035485	1	0
1	2000000417	-1340594121368301723	1	3
1	2000000418	-6617398307302016737	1	2
1	2000000419	7377611215149686245	1	4
1	2000000420	-8717890372998608465	1	1
1	2000000421	-3768139355512319682	1	2
1	2000000422	186989653214630812	1	1
1	2000000423	-8575991715294204431	1	1
1	2000000424	-2135398279124016704	1	1
1	2000000425	3745991302157209440	1	0
1	2000000426	7200778840955335664	1	0
1	2000000427	5403114043803834687	1	1
1	2000000428	7471078443531567468	1	0
1	2000000429	-6516337435024933937	1	0
1	2000000430	-8157418118392356068	1	0
1	2000000431	-8941669507633053478	1	1
1	2000000432	1750220706064829111	1	0
1	2000000433	-323380473992954954	1	0
1	2000000434	-4750834435677326447	1	0
1	2000000435	6395554917994421796	1	2
1	2000000436	-2393752537121269767	1	0
1	2000000437	-6232339777137704434	1	0
1	2000000438	-520295747944787748	1	0
1	2000000439	8935027621324685126	1	0
1	2000000440	1819288246643318899	1	0
1	2000000441	9082917390085156165	1	0
1	2000000442	2677094157800687234	1	0
1	2000000443	-5118235684681422184	1	0
1	2000000444	-7672359881715126002	1	0
1	2000000445	-3274095225854377694	1	2
1	2000000446	4773013725081228387	1	0
1	2000000447	8295892158328255943	1	0
1	2000000448	5946293114760361894	1	1
1	2000000449	-2857515491886374742	1	1
1	2000000450	-9016000243843539149	1	0
1	2000000451	7991943861722550330	1	1
1	2000000452	-5032889823836522363	1	1
1	2000000453	-1069215540778290849	1	0
1	2000000454	7933044701474115079	1	0
1	2000000455	3383483138513852096	1	0
1	2000000456	-5021294807930804702	1	0
1	2000000457	3232398040969668304	1	0
1	2000000458	5701699701766514880	1	3
1	2000000459	1079777846125395147	1	2
1	2000000460	-4757185636571062174	1	1
1	2000000461	631308306954226985	1	129
1	2000000462	6592849061380933631	1	1
1	2000000463	100475979119385272	1	0
1	2000000464	4488047058959874712	1	1
1	2000000465	6235711471212629325	1	0
1	2000000466	3450623954380032235	1	0
1	2000000467	-7805929135540215937	1	0
1	2000000468	-2912815792102701509	1	0
1	2000000469	-3149577814460868489	1	0
1	2000000470	2419264815177948334	1	0
1	2000000471	2892885172302753480	1	0
1	2000000472	2609711005454547867	1	0
1	2000000473	-517343203919092306	1	0
1	2000000474	-9124560809556394670	1	0
1	2000000475	-8330373889145914487	1	0
1	2000000476	8747320628437539152	1	0
1	2000000477	-6945858700441151447	1	0
1	2000000478	-6081354969982681999	1	0
1	2000000479	4035584893480932499	1	0
1	2000000480	-7056374640302649604	1	0
1	2000000481	-4791221260792473098	1	0
1	2000000482	-5333581857906755938	1	0
1	2000000483	657025011518556863	1	0
1	2000000484	-8970379998755300367	1	0
1	2000000485	-7040402825184554156	1	0
1	2000000486	-1460346436665267622	1	0
1	2000000487	-9022769976017791560	1	0
1	2000000488	8308133133012897805	1	0
1	2000000489	-5498163394167153218	1	0
1	2000000490	5204670132024648480	1	1
1	2000000491	-5006680664044913737	1	0
1	2000000492	8218793679501148847	1	0
1	2000000493	-3184534676668196556	1	0
1	2000000494	-1507875266406929359	1	0
1	2000000495	6412966820676540751	1	0
1	2000000496	5746705507760678982	1	0
1	2000000497	-1400974169623107081	1	0
1	2000000498	6386329771030373937	1	0
1	2000000499	1929673210871152925	1	0
1	2000000500	302047598451694000	1	11
1	2000000501	2062383995271269786	1	0
1	2000000502	-3325137484275582249	1	0
1	2000000503	3720236562660988131	1	0
1	2000000504	5455646959622234629	1	0
1	2000000505	-8946073386512960103	1	0
1	2000000506	-6933499149927628326	1	0
1	2000000507	3619311731473916220	1	0
1	2000000508	-3540429236196209402	1	0
1	2000000509	1680103686424152264	1	1
1	2000000510	5550703416682629513	1	1
1	2000000511	-1261190560869627214	1	0
1	2000000512	-7382620743575113589	1	0
1	2000000513	-6481782723578544090	1	0
1	2000000514	-6271626231804380759	1	0
1	2000000515	-2362168989856320897	1	0
1	2000000516	-3747520104157667041	1	0
1	2000000517	-6274934036837929527	1	0
1	2000000518	9063175315430654317	1	0
1	2000000519	3600368723188857852	1	0
1	2000000520	-3275278053552906791	1	0
1	2000000521	-4089443421389216307	1	0
1	2000000522	2209504073079976925	1	0
1	2000000523	6742207834329338799	1	0
1	2000000524	-1931136746910890635	1	0
1	2000000525	-8923877033546041150	1	2
1	2000000526	4361595946815463385	1	1
1	2000000527	-7126657288987151105	1	2
1	2000000528	-8474275296072444440	1	0
1	2000000529	-829046258088547010	1	0
1	2000000530	-3429772387199373398	1	0
1	2000000531	4287622489691514106	1	0
1	2000000532	4305927349006483485	1	0
1	2000000533	-5891975601941594263	1	0
1	2000000534	3552664310779555538	1	0
1	2000000535	77098920388313739	1	0
1	2000000536	1008163528827344863	1	0
1	2000000537	2378735368864199773	1	0
1	2000000538	-411873028061658547	1	0
1	2000000539	-4181691547645504898	1	0
1	2000000540	375420428497839569	1	0
1	2000000541	-4814854482884217950	1	0
1	2000000542	-5052095623130965223	1	0
1	2000000543	8665316472139466444	1	0
1	2000000544	-2352759059289503719	1	0
1	2000000545	-9185417756159682726	1	0
1	2000000546	-2537876146835428001	1	0
1	2000000547	1988307758015769345	1	0
1	2000000548	7375152660932761054	1	0
1	2000000549	-2204572513767827810	1	0
1	2000000550	-4083001903083056848	1	0
1	2000000551	-6417616407480563538	1	0
1	2000000552	-1090872573128234889	1	1
1	2000000553	-1870455361179779121	1	0
1	2000000554	-3911209253402778809	1	0
1	2000000555	1819032009935960307	1	0
1	2000000556	-7772786161986220840	1	0
1	2000000557	2887177096307434411	1	0
1	2000000558	-8330862128140684673	1	0
1	2000000559	-3752822565344380246	1	0
1	2000000560	-2642196237391216400	1	0
1	2000000561	-7514727290563466446	1	0
1	2000000562	-8803297981895917015	1	0
1	2000000563	1377339183922792491	1	0
1	2000000564	4139768908245332268	1	0
1	2000000565	-7258685998976334159	1	0
1	2000000566	8476909923968604590	1	0
1	2000000567	-6157168172706026864	1	0
1	2000000568	-3602472103318952577	1	0
1	2000000569	-6377809419111902802	1	0
1	2000000570	5629305678908643644	1	0
1	2000000571	1466672490978449442	1	0
1	2000000572	3936942997757393197	1	0
1	2000000573	-4021020179368974406	1	0
1	2000000574	-6841055803091445449	1	0
1	2000000575	3254722748319759749	1	0
1	2000000576	8770879149278552638	1	0
1	2000000577	-547175503758867039	1	13
1	2000000578	8267360228842387378	1	14
1	2000000579	-8840173634241471262	1	1
1	2000000580	-5353300839657544953	1	0
1	2000000581	-674673763740317037	1	0
1	2000000582	-316447274411310824	1	0
1	2000000583	-1591481693817104529	1	0
1	2000000584	2223566409522421057	1	0
1	2000000585	-1899365803510401341	1	0
1	2000000586	-1203559981284246877	1	0
1	2000000587	5783387881951179883	1	0
1	2000000588	8035979234755379503	1	0
1	2000000589	158014887412681138	1	0
1	2000000590	108268604432865115	1	0
1	2000000591	4027233132685793832	1	0
1	2000000592	-8836624144465935612	1	0
1	2000000593	224841885053618169	1	0
1	2000000594	-473344733180064987	1	0
1	2000000595	7303097711185187609	1	0
1	2000000596	6410203933308010749	1	0
1	2000000597	7144604583276889671	1	0
1	2000000598	-923832771737137593	1	0
1	2000000599	472257259985170296	1	0
1	2000000600	-7183873401417815368	1	0
1	2000000601	5639561393726250596	1	0
1	2000000602	-748429510391056660	1	0
1	2000000603	-1471821691389305120	1	0
1	2000000604	-1244110861695361205	1	0
1	2000000605	1849356259714524830	1	0
1	2000000606	5372648116563306155	1	0
1	2000000607	3788322937277224048	1	0
1	2000000608	1741724177889364421	1	0
1	2000000609	-1544374647241629962	1	0
1	2000000610	2270205132328204056	1	0
1	2000000611	4570899514858431606	1	0
1	2000000612	-1151592955510978112	1	0
1	2000000613	3641081747835851970	1	0
1	2000000614	3618095136331371696	1	0
1	2000000615	-6147070407927747851	1	0
1	2000000616	-8482464526912559302	1	0
1	2000000617	1454769167281619589	1	0
1	2000000618	-7160657270513693954	1	0
1	2000000619	-5707501221620982254	1	0
1	2000000620	-3393924028984365802	1	0
1	2000000621	218113030405659023	1	0
1	2000000622	-7326307373869845509	1	1
1	2000000623	8994507315322889238	1	0
1	2000000624	7113924106942583828	1	0
1	2000000625	-1178101138277602910	1	0
1	2000000626	6683456173721167212	1	0
1	2000000627	-5187881189794042804	1	0
1	2000000628	-5866543025278487313	1	1
1	2000000629	-1733326771329823463	1	0
1	2000000630	-7883477971508375745	1	0
1	2000000631	7861568514571853282	1	1
1	2000000632	3976811048074606695	1	0
1	2000000633	-7657782823826474449	1	1
1	2000000634	8033395148388329747	1	1
1	2000000635	7521579517602025726	1	0
1	2000000636	5133758865275727318	1	0
1	2000000637	353410713350147686	1	0
1	2000000638	4370706321304864293	1	0
1	2000000639	-604599232154294899	1	0
1	2000000640	4200379726516780142	1	0
1	2000000641	-4856317104683875529	1	0
1	2000000642	2948843064339980442	1	0
1	2000000643	5226501621194592299	1	0
1	2000000644	-3749193557444209145	1	0
1	2000000645	-3068262110343687907	1	0
1	2000000646	-7371290191581905607	1	0
1	2000000647	-1497134584702649418	1	0
1	2000000648	4836779721170942861	1	0
1	2000000649	2330984446180266726	1	1
1	2000000650	-4941497842926012142	1	1
1	2000000651	3850299087064257896	1	0
1	2000000652	1379885231265209318	1	0
1	2000000653	4730900279908313531	1	0
1	2000000654	-6692942897719607452	1	0
1	2000000655	6596385218658280824	1	0
1	2000000656	7742280597781519260	1	0
1	2000000657	-4939379533109856050	1	0
1	2000000658	-559271542322089807	1	0
1	2000000659	-4988170037811856119	1	0
1	2000000660	-1642963215527835214	1	0
1	2000000661	160470546533425842	1	0
1	2000000662	-4597536811667411047	1	0
1	2000000663	-5225835116227179315	1	0
1	2000000664	2012674202134181162	1	0
1	2000000665	9219722906344172547	1	0
1	2000000666	3789344299255261546	1	0
1	2000000667	-1110379169338656866	1	0
1	2000000668	-3259485908059675943	1	0
1	2000000669	7433331124027422644	1	0
1	2000000670	7999208031922245120	1	1
1	2000000671	5805668549733399878	1	1
1	2000000672	3314571711313360320	1	0
1	2000000673	-3548259621211206315	1	0
1	2000000674	4746829911096675271	1	0
1	2000000675	-7623144499908919361	1	0
1	2000000676	-6723657192789197229	1	0
1	2000000677	-6335971593227921026	1	0
1	2000000678	-5775568776361052001	1	0
1	2000000679	-8070008019278497384	1	0
1	2000000680	5679072309791352125	1	0
1	2000000681	8876830551412023741	1	0
1	2000000682	-3460807262607593561	1	0
1	2000000683	3293389237072111462	1	0
1	2000000684	-6436209041997647296	1	0
1	2000000685	6548174634444797682	1	0
1	2000000686	-8522120391317385162	1	0
1	2000000687	777704923058060297	1	0
1	2000000688	-900840466130694104	1	0
1	2000000689	4832504558073963968	1	0
1	2000000690	-8381922247839435305	1	0
1	2000000691	-704062481181072710	1	0
1	2000000692	2020796491414859841	1	0
1	2000000693	-2290156110695935850	1	0
1	2000000694	2302167374604391005	1	0
1	2000000695	-5674698766906137194	1	0
1	2000000696	-9183290367865991264	1	0
1	2000000697	7498069002045135382	1	0
1	2000000698	8894193892046569413	1	0
1	2000000699	-7704236595691425209	1	0
1	2000000700	7579344795684748176	1	0
1	2000000701	6996668969732783144	1	0
1	2000000702	-8009200561564919891	1	0
1	2000000703	5603297846530487325	1	0
1	2000000704	3274810119064705259	1	0
1	2000000705	3146147149573717354	1	7
1	2000000706	-2228400562854313153	1	0
1	2000000707	-8781065821573446522	1	1
1	2000000708	-6650893164806474862	1	0
1	2000000709	-3532027586826254054	1	0
1	2000000710	-7076876310571766028	1	1
1	2000000711	-7871678614102366785	1	0
1	2000000712	-947369421393944280	1	0
1	2000000713	-7249504719250979845	1	0
1	2000000714	5163656879946869082	1	1
1	2000000715	-6689405592075286178	1	0
1	2000000716	2041071870319067295	1	0
1	2000000717	7315194354421844525	1	1
1	2000000718	-1981447961786148634	1	0
1	2000000719	-7761558375622485442	1	2
1	2000000720	2847239473139498991	1	0
1	2000000721	8511227886358174355	1	0
1	2000000722	8663491027335178694	1	0
1	2000000723	-7082171019313824030	1	0
1	2000000724	-6964947218939580574	1	0
1	2000000725	1566535363509039560	1	1
1	2000000726	-5864548989101485226	1	0
1	2000000727	-8884838108438765824	1	0
1	2000000728	-6892007837533608991	1	9
1	2000000729	6640252121598649676	1	1
1	2000000730	6974283897199924901	1	0
1	2000000731	-2413253437294506737	1	1
1	2000000732	-5745046589739276255	1	1
1	2000000733	-2816095407172438235	1	0
1	2000000734	-1526317184543969641	1	0
1	2000000735	-4236279152836641538	1	1
1	2000000736	5883450169777154787	1	1
1	2000000737	-8885949884843028693	1	0
1	2000000738	-4275480352121319611	1	1
1	2000000739	4464399667694645336	1	1
1	2000000740	-8230243735332585566	1	0
1	2000000741	3843506714796885343	1	0
1	2000000742	-6438327568176191801	1	0
1	2000000743	-936893693115527437	1	0
1	2000000744	-8722452410758653635	1	0
1	2000000745	9175510766633525267	1	0
1	2000000746	407113350092855760	1	0
1	2000000747	190569239989277010	1	0
1	2000000748	-1675329261346332849	1	0
1	2000000749	-6997337568353676874	1	0
1	2000000750	5400288476079911027	1	7
1	2000000751	-5094202754092917760	1	1
1	2000000752	-6281351739100713367	1	0
1	2000000753	-8356487029510250283	1	0
1	2000000754	-2768266330783356096	1	0
1	2000000755	2991194027017732164	1	0
1	2000000756	6817250711474100479	1	0
1	2000000757	-8609459946517706479	1	0
1	2000000758	1117597016868852500	1	2
1	2000000759	5480063571633879688	1	0
1	2000000760	-786012976648189977	1	0
1	2000000761	-7867237249420254429	1	0
1	2000000762	78506733819387106	1	0
1	2000000763	7343414712412368710	1	1
1	2000000764	-3616245965538611598	1	43
1	2000000765	9062243209505088646	1	1
1	2000000766	-5589140067204984410	1	0
1	2000000767	-4620830751155899334	1	0
1	2000000768	-4685899739859365940	1	0
1	2000000769	5988353012999221631	1	2
1	2000000770	1625551037688455344	1	1
1	2000000771	4612079968168220183	1	0
1	2000000772	-8903470396850240656	1	0
1	2000000773	7471397317073184649	1	0
1	2000000774	-9073260828057306664	1	0
1	2000000775	-6004689372673379646	1	0
1	2000000776	-638364817117994440	1	2
1	2000000777	-2322817597090801987	1	2
1	2000000778	-6912443921216102974	1	0
1	2000000779	-5206855240369371018	1	1
1	2000000780	3301255963549947634	1	1
1	2000000781	4215354479734997830	1	0
1	2000000782	6581433749055524307	1	1
1	2000000783	5546863820523493479	1	1
1	2000000784	255751341649350614	1	1
1	2000000785	-6627260910814133887	1	1
1	2000000786	1451008053836700258	1	0
1	2000000787	5912273332061696898	1	1
1	2000000788	4914174051134168774	1	1
1	2000000789	-1505864365404196627	1	0
1	2000000790	-727451794362875038	1	0
1	2000000791	2894744210331295187	1	0
1	2000000792	1772480079821854380	1	0
1	2000000793	6535182933820282663	1	0
1	2000000794	5501484454222036205	1	0
1	2000000795	665505556097720381	1	0
1	2000000796	5086047834088109126	1	0
1	2000000797	-1140359889630538607	1	0
1	2000000798	7731012049117049076	1	0
1	2000000799	4038449796716148440	1	0
1	2000000800	-2664297099276952766	1	3
1	2000000801	-2482500879906466549	1	1
1	2000000802	2425597280237009217	1	1
1	2000000803	-586652006490389178	1	1
1	2000000804	6580682854756946552	1	1
1	2000000805	686799993030402996	1	1
1	2000000806	4874361422224701319	1	0
1	2000000807	8502745391283177618	1	0
1	2000000808	8614401276580995037	1	0
1	2000000809	-1327063142942510431	1	0
1	2000000810	1932581422101276016	1	1
1	2000000811	-2291857299264558162	1	1
1	2000000812	-4080222699972974441	1	7
1	2000000813	7720997577829840202	1	0
1	2000000814	3408792733589247099	1	0
1	2000000815	8691023200992283088	1	0
1	2000000816	-242433313724515810	1	1
1	2000000817	2673070882760588525	1	39
1	2000000818	3049192474232067376	1	0
1	2000000819	-2575370679915686837	1	1
1	2000000820	-3076343116555089877	1	0
1	2000000821	5287458118285883886	1	0
1	2000000822	694918346179025770	1	0
1	2000000823	-6342531851170206187	1	0
1	2000000824	-1692105356781825231	1	0
1	2000000825	-992377258616668112	1	0
1	2000000826	-6311014117888634806	1	0
1	2000000827	4848000494819026557	1	0
1	2000000828	1059691320582458907	1	0
1	2000000829	-3032344655395207967	1	0
1	2000000830	-2372598623342294744	1	0
1	2000000831	5425160292901546755	1	0
1	2000000832	76488856091185374	1	1
1	2000000833	1224547687312934289	1	0
1	2000000834	-4155611304333476112	1	0
1	2000000835	-5570697310300715893	1	0
1	2000000836	-4576905349066182435	1	0
1	2000000837	7610556504345267071	1	0
1	2000000838	-7470257043996736661	1	0
1	2000000839	416687423359059407	1	0
1	2000000840	7228942233047580204	1	0
1	2000000841	4546933048618210470	1	0
1	2000000842	-3440651093663044021	1	1
1	2000000843	-292277682007308740	1	0
1	2000000844	4324309647875998771	1	0
1	2000000845	-1178792961506915486	1	0
1	2000000846	-1452112783999086304	1	0
1	2000000847	4208383395991285259	1	1
1	2000000848	2681240363186309918	1	0
1	2000000849	5948516854699542069	1	0
1	2000000850	-8282321649916488413	1	0
1	2000000851	-7870819632915365856	1	3
1	2000000852	7436420335013592145	1	1
1	2000000853	-2047916398335965857	1	0
1	2000000854	-9015728697059075019	1	0
1	2000000855	728247740357176199	1	0
1	2000000856	-9165043103091401601	1	0
1	2000000857	-3020579081032030055	1	1
1	2000000858	4413574436139789906	1	0
1	2000000859	8388470501585506122	1	1
1	2000000860	-4875998285682959509	1	0
1	2000000861	-8011489024666563166	1	0
1	2000000862	4308079095349473247	1	0
1	2000000863	1091469637333404721	1	0
1	2000000864	-4473303655182815444	1	0
1	2000000865	-2275697846518862928	1	2
1	2000000866	-9184799163675668790	1	0
1	2000000867	4057932499337166717	1	0
1	2000000868	-6006606298310346554	1	0
1	2000000869	-7255101302504907991	1	0
1	2000000870	4712272820753174645	1	1
1	2000000871	-7673554198257650506	1	0
1	2000000872	-7262478061143821468	1	0
1	2000000873	5858929025123377530	1	0
1	2000000874	-8351617673807613926	1	0
1	2000000875	-6339998741219588628	1	0
1	2000000876	-1930517508288140553	1	0
1	2000000877	-5004865016691376838	1	0
1	2000000878	-1229892140439693211	1	0
1	2000000879	-7521650831978226364	1	1
1	2000000880	4176625386367073818	1	0
1	2000000881	4794068930792434294	1	0
1	2000000882	-4988149378914569423	1	1
1	2000000883	-6860337138445249149	1	0
1	2000000884	3964857468385941503	1	0
1	2000000885	-4989319812405585052	1	0
1	2000000886	1742266396999405263	1	0
1	2000000887	3886518409372390089	1	0
1	2000000888	-1135443937584918265	1	1
1	2000000889	2006855055502907474	1	0
1	2000000890	-768227824610522298	1	0
1	2000000891	4716354392741012469	1	0
1	2000000892	-3666150636673913382	1	1
1	2000000893	5742032320271158230	1	0
1	2000000894	8039513648741184249	1	0
1	2000000895	-4757149717594373648	1	0
1	2000000896	912800248653254451	1	0
1	2000000897	2171890429284314195	1	0
1	2000000898	6104628488879558047	1	1
1	2000000899	-3467873180011179379	1	0
1	2000000900	-5525605133353891528	1	0
1	2000000901	-6346860121539058337	1	0
1	2000000902	4814594667029517543	1	0
1	2000000903	544509640253020970	1	2
1	2000000904	265829263482519944	1	1
1	2000000905	44681976019897786	1	0
1	2000000906	-5744614909404880627	1	0
1	2000000907	5835430415156872082	1	1
1	2000000908	3465093300269864384	1	1
1	2000000909	-3078742353904403650	1	0
1	2000000910	882326949164834635	1	0
1	2000000911	7664680734285423530	1	0
1	2000000912	883003652861180376	1	0
1	2000000913	-7293576720655050036	1	1
1	2000000914	7923350681241079024	1	1
1	2000000915	-2056343294866171643	1	0
1	2000000916	9081816143670670034	1	0
1	2000000917	-967846679142506426	1	0
1	2000000918	-7204819334085466013	1	0
1	2000000919	5880049292494101918	1	1
1	2000000920	8221084992272232112	1	1
1	2000000921	609604938207610433	1	0
1	2000000922	-347706343151557748	1	0
1	2000000923	-4569641039552417056	1	1
1	2000000924	-4976273116129123078	1	0
1	2000000925	-2625437675942997275	1	0
1	2000000926	7729616558499493031	1	0
1	2000000927	3836076711622384050	1	0
1	2000000928	-8805606788410929812	1	0
1	2000000929	63851718022001911	1	0
1	2000000930	9205696324497013	1	0
1	2000000931	8724533709734202211	1	0
1	2000000932	256728256771641962	1	0
1	2000000933	-1131743231451411598	1	0
1	2000000934	-2449078386832694601	1	1
1	2000000935	6064964023703205388	1	1
1	2000000936	8966706877660743097	1	0
1	2000000937	-4025443318567146955	1	0
1	2000000938	5485868942381942023	1	0
1	2000000939	5879881600104610197	1	0
1	2000000940	-2939408616828985634	1	0
1	2000000941	5406123222108191294	1	0
1	2000000942	4343929996627703905	1	0
1	2000000943	-7672027769739477145	1	0
1	2000000944	7437103956357837389	1	0
1	2000000945	7915545078928575986	1	0
1	2000000946	-8545790135179170598	1	0
1	2000000947	701031601927376825	1	1
1	2000000948	5888195715833119222	1	0
1	2000000949	-9112096851723170186	1	0
1	2000000950	3384374749149532045	1	0
1	2000000951	-327349863373322152	1	0
1	2000000952	-6813084329999632373	1	0
1	2000000953	-7917844593539014293	1	0
1	2000000954	-3560994649842946011	1	0
1	2000000955	2367462802463363422	1	0
1	2000000956	-3613740110692547712	1	0
1	2000000957	6589356139030126645	1	0
1	2000000958	3528086416049257714	1	2
1	2000000959	7071521955825074980	1	0
1	2000000960	4670464998368903761	1	0
1	2000000961	8052128727758468466	1	0
1	2000000962	-4027489574189355398	1	0
1	2000000963	-7772401465520405793	1	0
1	2000000964	-7864230203590923057	1	1
1	2000000965	8910539476105716733	1	0
1	2000000966	-4928145995967954691	1	0
1	2000000967	-986376273584432818	1	2
1	2000000968	-4640904247616785874	1	0
1	2000000969	8190960998390422427	1	0
1	2000000970	-4608901660714913337	1	0
1	2000000971	1275011559545855911	1	0
1	2000000972	-1295201336329561053	1	1
1	2000000973	9091829820384950553	1	2
1	2000000974	6540957371066614316	1	0
1	2000000975	-2019419135082391874	1	0
1	2000000976	-2398904949545213388	1	0
1	2000000977	-9190790590285646942	1	0
1	2000000978	7849459939427238065	1	0
1	2000000979	-8299700317371339995	1	1
1	2000000980	-377606298713362625	1	0
1	2000000981	-2031725293327449026	1	1
1	2000000982	5983264011791497714	1	1
1	2000000983	-7690610360618111555	1	0
1	2000000984	449937141918057277	1	0
1	2000000985	6914732839229375633	1	0
1	2000000986	-8850891524784861637	1	0
1	2000000987	8533968584872633970	1	1
1	2000000988	681146999070498170	1	1
1	2000000989	-3450014328039069239	1	0
1	2000000990	-8342820603299250897	1	0
1	2000000991	-1091438705083176208	1	0
1	2000000992	5902272033214131380	1	1
1	2000000993	7908348201613442623	1	1
1	2000000994	-5149731834386805752	1	0
1	2000000995	-1512542050362921247	1	0
1	2000000996	-2555596903551353834	1	0
1	2000000997	-8591353267454696957	1	0
1	2000000998	4648438223477796593	1	1
1	2000000999	2245323675258877451	1	0
1	2000001000	6959475857629491259	1	0
1	2000001001	-7125449764991703689	1	44
1	2000001002	-2806368011398906520	1	0
1	2000001003	5594314531942316630	1	0
1	2000001004	-8988012340689947584	1	0
1	2000001005	-4528637836425608369	1	0
1	2000001006	-1135189464947323845	1	0
1	2000001007	2885526710536598113	1	1
1	2000001008	1286238108062068368	1	0
1	2000001009	-6648944238338842481	1	1
1	2000001010	6860362571554215216	1	0
1	2000001011	321554501983926968	1	0
1	2000001012	5367931265214041181	1	1
1	2000001013	3538872270517270634	1	0
1	2000001014	7732139213757531631	1	0
1	2000001015	6714215559856081395	1	1
1	2000001016	-7641392127491465904	1	0
1	2000001017	2276997983505728369	1	2
1	2000001018	-1007467999953333188	1	2
1	2000001019	2374965325163934168	1	3
1	2000001020	5616213889778005441	1	6
1	2000001021	8455353873766678633	1	5
1	2000001022	-8398102793415941379	1	4
1	2000001023	-2780071311272975110	1	2
1	2000001024	-8053000820966812681	1	5
1	2000001025	-6664754518424090642	1	8
1	2000001026	-6570008418861600440	1	4
1	2000001027	6551778862476105365	1	14
1	2000001028	6627834638240895512	1	19
1	2000001029	8315107481872866927	1	28
1	2000001030	5670771697453772560	1	1
1	2000001031	1544155196440893856	1	0
1	2000001032	-4770951617551334686	1	2
1	2000001033	-372345773148852929	1	0
1	2000001034	-5733967213181359467	1	2
1	2000001035	-4176918439774775822	1	4
1	2000001036	5691238000848924670	1	1
1	2000001037	-3085202136643791384	1	16
1	2000001038	-6131068197541576738	1	3
1	2000001039	-6862115297513520503	1	2
1	2000001040	-464283600867059024	1	5
1	2000001041	7186828315059264889	1	9
1	2000001042	6158124222323029640	1	2
1	2000001043	-8464602885554311459	1	1
1	2000001044	108747157671611731	1	1
1	2000001045	8462740860802420442	1	1
1	2000001046	-2399654172080959678	1	23
1	2000001047	-8910952457536193267	1	0
1	2000001048	-6906691816955658071	1	1
1	2000001049	-3635710217030240506	1	3
1	2000001050	5857327834671419286	1	12
1	2000001051	-5928123676542923322	1	0
1	2000001052	-1222368133678978019	1	0
1	2000001053	5269657952108192472	1	1
1	2000001054	2067208644914676018	1	12
1	2000001055	-2638291134221394996	1	14
1	2000001056	2153771988474078008	1	8
1	2000001057	-960128883284789234	1	2
1	2000001058	2460692043327998509	1	4
1	2000001059	-367814829649005664	1	10
1	2000001060	1237306865968645953	1	7
1	2000001061	-768449131088247831	1	6
1	2000001062	-891949923407065854	1	3
1	2000001063	-7458424375290572939	1	4
1	2000001064	5898754603752214121	1	3
1	2000001065	6447188493202147399	1	9
1	2000001066	8522311862062046826	1	7
1	2000001067	3009239510960794656	1	8
1	2000001068	4233681671533271443	1	40
1	2000001069	-288608372800445033	1	4
1	2000001070	-1688916975541725591	1	9
1	2000001071	4230669388504180511	1	3
1	2000001072	-1441439608458833047	1	1
1	2000001073	-8693463573350018352	1	0
1	2000001074	-6822581025649830660	1	0
1	2000001075	5299779084982604411	1	36
1	2000001076	7341252202662817654	1	3
1	2000001077	-3734011219728813011	1	3
1	2000001078	-3111047399857813227	1	13
1	2000001079	6356044858478115513	1	4
1	2000001080	7631961285996600547	1	3
1	2000001081	4078814598570768663	1	31
1	2000001082	-2671902171571531285	1	5
1	2000001083	5121651198702756892	1	6
1	2000001084	87388039916460421	1	5
1	2000001085	911759618204730973	1	4
1	2000001086	4291486113038902560	1	5
1	2000001087	-6881348954402300356	1	5
1	2000001088	-6709824203167960709	1	5
1	2000001089	-8128155084156210464	1	5
1	2000001090	-2453704373727120087	1	4
1	2000001091	8257310182211849555	1	4
1	2000001092	2005531911630964251	1	7
1	2000001093	-8131967445052921272	1	4
1	2000001094	-52907678272727815	1	4
1	2000001095	7116507851557313248	1	4
1	2000001096	-7215120721757903283	1	6
1	2000001097	-8960805452828810794	1	5
1	2000001098	-7067785902223299339	1	4
1	2000001099	-1763050638754675372	1	0
1	2000001100	5780901546479079195	1	2
1	2000001101	5524856594142199329	1	1
1	2000001102	3863217587438177010	1	1
1	2000001103	-6530690482946375377	1	2
1	2000001104	5437554715955851851	1	1
1	2000001105	3211889679154096856	1	9
1	2000001106	-2395467479759687223	1	0
1	2000001107	-2573991658597989803	1	0
1	2000001108	-4377735459434946599	1	1
1	2000001109	-72027690974534248	1	0
1	2000001110	207189933907901365	1	0
1	2000001111	2361921548870230688	1	0
1	2000001112	-3252907002280195029	1	4
1	2000001113	4604669814823382222	1	0
1	2000001114	-6435126593051665603	1	4
1	2000001115	7282032306123914706	1	7
1	2000001116	848326468934320527	1	5
1	2000001117	9190602656340832148	1	2
1	2000001118	-6473671706617561918	1	12
1	2000001119	-5726951638775068934	1	1
1	2000001120	-9079328690913641210	1	0
1	2000001121	-9210625787600764149	1	0
1	2000001122	4513977049716890332	1	4
1	2000001123	-9158713616993224031	1	3
1	2000001124	4374512322371477589	1	12
1	2000001125	6357542954624213120	1	2
1	2000001126	7547618722826960891	1	5
1	2000001127	-8143904735275224676	1	1
1	2000001128	-1306018922829213234	1	1
1	2000001129	3842414158465501632	1	2
1	2000001130	3670611775151130911	1	19
1	2000001131	2029903199701682342	1	2
1	2000001132	5516171768802322427	1	0
1	2000001133	8345614292201443968	1	0
1	2000001134	-8879581351573486110	1	0
1	2000001135	8605820832777887285	1	1
1	2000001136	-3966573541100699617	1	1
1	2000001137	3228696117376486889	1	19
1	2000001138	-4996633728173344823	1	0
1	2000001139	6676658122434186560	1	0
1	2000001140	971174339332451099	1	7
1	2000001141	-7177800226208002436	1	0
1	2000001142	307236824694356492	1	0
1	2000001143	2550114325481514518	1	0
1	2000001144	4637543035199056940	1	16
1	2000001145	-6230743295456017513	1	11
1	2000001146	7192667341691898816	1	12
1	2000001147	5205638624216220967	1	4
1	2000001148	-6565075019170510868	1	4
1	2000001149	3782058628936311715	1	1
1	2000001150	-3195672186131227094	1	52
1	2000001151	-5334509675145412683	1	1
1	2000001152	7016315370034764348	1	2
1	2000001153	6476892859771650470	1	1
1	2000001154	-3465740592744057548	1	16
1	2000001155	-3037517196981363286	1	8
1	2000001156	1114369319716674198	1	7
1	2000001157	5751062995806576286	1	8
1	2000001158	236766866536675906	1	5
1	2000001159	-1944632089074046664	1	11
1	2000001160	-7752258273955670937	1	3
1	2000001161	-2928071068988228475	1	2
1	2000001162	4994927038042679865	1	3
1	2000001163	8281097770071769022	1	3
1	2000001164	-2960412080906415157	1	8
1	2000001165	-8762168969870978127	1	2
1	2000001166	2243133368614220796	1	5
1	2000001167	1715240283461579933	1	3
1	2000001168	9219794399687963470	1	3
1	2000001169	-804832200965223130	1	1
1	2000001170	-6364009762563688071	1	5
1	2000001171	1816866295334011362	1	5
1	2000001172	3503386237523232980	1	4
1	2000001173	-2297595275933006673	1	4
1	2000001174	-4392497747760791413	1	3
1	2000001175	-1780397812148861499	1	3
1	2000001176	-3264790370237890194	1	2
1	2000001177	-4578522483913369008	1	0
1	2000001178	-8685499480915168784	1	0
1	2000001179	8991301413776457540	1	0
1	2000001180	7145918418903829447	1	1
1	2000001181	-662693263978555887	1	7
1	2000001182	5179246638415980096	1	5
1	2000001183	364245053286702444	1	324
1	2000001184	-6089330072891960682	1	2
1	2000001185	4974840309390420475	1	0
1	2000001186	-6891234349198819717	1	1
1	2000001187	-1081932165472392682	1	0
1	2000001188	-862757893900596786	1	0
1	2000001189	4761756117214222146	1	0
1	2000001190	-7575432177193452195	1	1
1	2000001191	-4702397548553202652	1	6
1	2000001192	45264677980430591	1	0
1	2000001193	3878448300163266785	1	4
1	2000001194	-9023608126608041850	1	0
1	2000001195	-4416009015091390470	1	0
1	2000001196	7866436034146456517	1	0
1	2000001197	-249521819635086411	1	0
1	2000001198	4501916349915969181	1	0
1	2000001199	2065468963683054448	1	0
1	2000001200	2495145267416411563	1	0
1	2000001201	2015705189481042425	1	0
1	2000001202	-8971579562146055686	1	0
1	2000001203	3703406014009492720	1	0
1	2000001204	7019895460183837475	1	2
1	2000001205	-2813586467317074384	1	0
1	2000001206	-2428216348344006560	1	0
1	2000001207	6553941132847579211	1	0
1	2000001208	-1557936242763417275	1	0
1	2000001209	7643520708485295897	1	0
1	2000001210	7056256915170179218	1	0
1	2000001211	906041612081580848	1	0
1	2000001212	2255057613336132639	1	0
1	2000001213	7239633883044694886	1	0
1	2000001214	3749794823157993345	1	0
1	2000001215	5190530051342802745	1	0
1	2000001216	4822238645918970741	1	0
1	2000001217	4695969812868151224	1	0
1	2000001218	-522817394540157663	1	0
1	2000001219	-2078955599824486635	1	0
1	2000001220	4171638246585978850	1	0
1	2000001221	1813219048352356292	1	0
1	2000001222	4554315221965242666	1	0
1	2000001223	-6833410158936175156	1	0
1	2000001224	-5296972718564795889	1	0
1	2000001225	-1269317059936514769	1	0
1	2000001226	-8468074403158276113	1	0
1	2000001227	7447131582808425736	1	0
1	2000001228	-1332485022344352573	1	0
1	2000001229	5082681135190863564	1	2
1	2000001230	3437836429829108529	1	1
1	2000001231	238092025138455498	1	185
1	2000001232	2849429986525720309	1	1
1	2000001233	1351459559490765744	1	1
1	2000001234	4483650246607868747	1	182
1	2000001235	-2589841752501854624	1	7
1	2000001236	2170570042553337662	1	83
1	2000001237	606864617872302992	1	233
1	2000001238	-2624186850303116539	1	219
1	2000001239	-6857719881763129813	1	1
1	2000001240	-561277564324188454	1	0
1	2000001241	691319980552543269	1	2
1	2000001242	1275445145094723242	1	1
1	2000001243	-3295972363196954376	1	4
1	2000001244	-91721315894732797	1	2
1	2000001245	3092181141534013781	1	0
1	2000001246	3693379232998328949	1	0
1	2000001247	-5922794219681273649	1	0
1	2000001248	1300457086185047770	1	0
1	2000001249	-361911890771219027	1	0
1	2000001250	1925544013814049554	1	0
1	2000001251	2634373055963632102	1	0
1	2000001252	-665776924063090015	1	185
1	2000001253	-5136452061916394765	1	856
1	2000001254	8897796258654925583	1	2654
1	2000001255	-7384987120366478110	1	860
1	2000001256	-5937758128597759100	1	92
1	2000001257	-1416300145659030343	1	2014
1	2000001258	4258774145876756265	1	610
1	2000001259	4223747915072276681	1	603
1	2000001260	-8824255370276270199	1	0
1	2000001261	3177323779712560580	1	0
1	2000001262	3983000563565984390	1	0
1	2000001263	-7000959205335803701	1	0
1	2000001264	-5005933384339086461	1	0
1	2000001265	2698705444242427190	1	183
1	2000001266	687857937432763853	1	0
1	2000001267	-5597116519052833346	1	139
1	2000001268	-2591488856581830582	1	63
1	2000001269	-6544790822615003457	1	475
1	2000001270	-2825558108184742708	1	471
1	2000001271	1168994223344323518	1	20
1	2000001272	-2032319083738454373	1	1
1	2000001273	1796867783899292493	1	0
1	2000001274	-3686252796771842845	1	1
1	2000001275	-2270158545202692171	1	1
1	2000001276	8730914225656170048	1	6
1	2000001277	-8215178805094666448	1	3
1	2000001278	-5291778478237688275	1	0
1	2000001279	1525258728227518028	1	0
1	2000001280	5453959019869486766	1	0
1	2000001281	-7847209498562172930	1	0
1	2000001282	823917457326975110	1	0
1	2000001283	-1036781908657052566	1	0
1	2000001284	7512737960426415814	1	0
1	2000001285	-2550841874831921724	1	0
1	2000001286	-6338598668737218660	1	0
1	2000001287	-327137777713093119	1	0
1	2000001288	9150871031119415570	1	0
1	2000001289	-684804499359966456	1	0
1	2000001290	-6157280172931092825	1	17
1	2000001291	-2737519622774481858	1	5
1	2000001292	1179132302154905766	1	3
1	2000001293	-3143703892714601300	1	3
1	2000001294	-6785070894776717730	1	3
1	2000001295	7748983039484611270	1	3
1	2000001296	4952726493009253463	1	2
1	2000001297	-1852319116449686914	1	2
1	2000001298	-8978907799821498318	1	3
1	2000001299	7959537034108505793	1	3
1	2000001300	-141851571656334866	1	2
1	2000001301	7626764079958247388	1	2
1	2000001302	7836664940262010163	1	2
1	2000001303	4776222105303431085	1	5
1	2000001304	2104993403377692519	1	3
1	2000001305	8524398317037426129	1	2
1	2000001306	-3020259999343376675	1	2
1	2000001307	-6410872747388853356	1	5
1	2000001308	-2734078166033749347	1	3
1	2000001309	696890939607808317	1	3
1	2000001310	-5248779109464195655	1	2
1	2000001311	3635981740501520847	1	2
1	2000001312	7868981472740619149	1	3
1	2000001313	-5382392792071956320	1	3
1	2000001314	4722228480134289451	1	4
1	2000001315	2258045652370874348	1	3
1	2000001316	7458238141391050739	1	2
1	2000001317	5324069311841853375	1	2
1	2000001318	7682350723153265293	1	2
1	2000001319	-255479049949798456	1	5
1	2000001320	8244428772910906594	1	0
1	2000001321	7802840229146774501	1	0
1	2000001322	2672924392723299654	1	0
1	2000001323	3420410409558794600	1	0
1	2000001324	-1879805963535627571	1	0
1	2000001325	7902977540104998009	1	0
1	2000001326	987872124572520775	1	0
1	2000001327	2486188709265545329	1	1
1	2000001328	-7223695806028118107	1	0
1	2000001329	-952303128732388034	1	7
1	2000001330	6220656556765335703	1	7
1	2000001331	-7168421973051444652	1	4
1	2000001332	-7929991792431856222	1	4
1	2000001333	-3009228473796984114	1	4
1	2000001334	7716124063300007096	1	4
1	2000001335	5536513000467519387	1	5
1	2000001336	2628421056183551099	1	5
1	2000001337	-92720985729499244	1	5
1	2000001338	-8226364803093571800	1	6
1	2000001339	-7741358946032307142	1	4
1	2000001340	6920604210740085850	1	4
1	2000001341	-1483961530525751665	1	2
1	2000001342	6213761762685107727	1	1
1	2000001343	-7601845516294979005	1	0
1	2000001344	-8937175801598297559	1	0
1	2000001345	2640566765996170294	1	0
1	2000001346	8144422378637737611	1	0
1	2000001347	8023933319764954538	1	0
1	2000001348	-8310467304190011765	1	0
1	2000001349	5122595439880943005	1	0
1	2000001350	-7367917403558354336	1	1
1	2000001351	8409376315647180451	1	6
1	2000001352	-9085754636720657457	1	0
1	2000001353	-614415171202939901	1	0
1	2000001354	-2457224510879870735	1	0
1	2000001355	-5248650230956437074	1	0
1	2000001356	3638941178979011375	1	0
1	2000001357	9116659678081060652	1	0
1	2000001358	7943931625662181984	1	14
1	2000001359	7765117299931483029	1	190
1	2000001360	-761294447628058446	1	46
1	2000001361	6057698298866825827	1	60
1	2000001362	5222284899394960018	1	45
101	2000000000	-5168067613992917628	1	112
101	2000000001	1658048510990355987	1	624
101	2000000002	395155716920624105	1	23
101	2000000003	2105868547303546195	1	1
101	2000000004	4168344068604504176	1	33
101	2000000005	-599379416200460755	1	4
101	2000000006	5865893109958142336	1	16
101	2000000007	-1014448920728114714	1	9
101	2000000008	-5914716010989158113	1	4
101	2000000009	4226214848212862725	1	2
101	2000000010	4142698781433296355	1	5
101	2000000011	891115598539416633	1	0
101	2000000012	-8247447631318334064	1	3
101	2000000013	5039604955451317509	1	1
101	2000000014	320849263338300981	1	0
101	2000000015	-5488696493139541078	1	0
101	2000000016	1097231189897963555	1	0
101	2000000017	-7740681692428656038	1	1
101	2000000018	6553368331669324775	1	0
101	2000000019	-8986029802410366237	1	1
101	2000000020	1605842430492936233	1	0
101	2000000021	104097758823551538	1	3
101	2000000022	5914280589009734834	1	3
101	2000000023	4213147678250946958	1	3
101	2000000024	6183954501267872216	1	0
101	2000000025	-5205656477440234433	1	1
101	2000000026	-7493652208583830495	1	3
101	2000000027	2908739203109759186	1	0
101	2000000028	5130518625500238005	1	0
101	2000000029	7882878838845453136	1	0
101	2000000030	6892766825010809023	1	0
101	2000000031	-5663271173113375380	1	1
101	2000000032	9154034748061152465	1	0
101	2000000033	-1357977716921926776	1	1
101	2000000034	-6629809875920628607	1	0
101	2000000035	-5312011533091652985	1	2
101	2000000036	718348107055543106	1	12
101	2000000037	-1157373951113888645	1	1
101	2000000038	3837923844096457385	1	11
101	2000000039	-7864049997018424615	1	0
101	2000000040	-2628665152080434732	1	0
101	2000000041	7784828775858321386	1	0
101	2000000042	8687322539772689954	1	0
101	2000000043	-8965823899831416089	1	0
101	2000000142	6165186261284916642	1	50
101	2000000143	5961349759012781409	1	216
101	2000000144	3358260765246435230	1	10
101	2000000145	3541929903117895034	1	7
101	2000000146	-1084510063449187158	1	10
101	2000000147	3457924317783329288	1	8
101	2000000148	8805228457669272158	1	2
101	2000000149	5062942869313641847	1	3
101	2000000150	8881830009048021057	1	11
101	2000000151	6762983883462077005	1	3
101	2000000152	-5747231119492549359	1	3
101	2000000153	3222081366833223268	1	9
101	2000000154	2466633683436661857	1	1
101	2000000155	-1575323095389685462	1	5
101	2000000156	-802959555982311160	1	1
101	2000000157	-2304382874982447364	1	16
101	2000000158	-7215277425977975246	1	1
101	2000000159	-1094041455508475148	1	1
101	2000000160	3421573324132374923	1	1
101	2000000161	-9207514556493929158	1	0
101	2000000162	4504696974646285517	1	0
101	2000000163	5780058753660829978	1	0
101	2000000164	5788044238413357566	1	7
101	2000000165	45366133087923521	1	3
101	2000000166	-3107720997913478708	1	0
101	2000000167	-7414640722507565204	1	29
101	2000000168	2728663142375709036	1	2
101	2000000169	5532191644254049702	1	1
101	2000000170	-2575506855031989672	1	0
101	2000000171	3913174321725019164	1	0
101	2000000172	-6325375061704290982	1	2
101	2000000173	1952829678854231830	1	0
101	2000000174	-7714358181202295009	1	0
101	2000000175	167381447430374963	1	84
101	2000000176	-7156862266043976418	1	23
101	2000000177	7453602060532121344	1	49
101	2000000178	2048672468661446044	1	0
101	2000000179	-5313316152668899945	1	0
101	2000000180	6546417840909422353	1	0
101	2000000181	7998291862378205807	1	0
101	2000000182	1583629675819367816	1	17
101	2000000183	6015330456467304167	1	2
101	2000000184	-3922533019978291516	1	1
101	2000000185	6469926083620759826	1	1
101	2000000186	-6197704211498601688	1	22
101	2000000187	7506018132291755046	1	8
101	2000000188	-2854098181133654390	1	1
101	2000000189	5580666055292867697	1	0
101	2000000190	-5586930227402413306	1	1
101	2000000191	-5863135281082852428	1	1
101	2000000192	6876094986674014987	1	17
101	2000000193	-4387068365724119821	1	1
101	2000000194	7685242590091901238	1	10
101	2000000195	-2846826406673728752	1	51
101	2000000196	9174150359974885957	1	58
101	2000000197	-2872144303876561253	1	19
101	2000000198	-3668471866226188888	1	56
101	2000000199	2286211212203948379	1	40
101	2000000200	3876868695864254709	1	40
101	2000000201	-8982279052801127019	1	20
101	2000000202	-1191290804749559924	1	31
101	2000000203	3188356211687288195	1	30
101	2000000204	8158340901452721225	1	133
101	2000000205	6159620885790173160	1	161
101	2000000206	3132744306913834556	1	11
101	2000000207	-3207103653603210855	1	19
101	2000000208	-8222704019872743726	1	10
101	2000000209	-8768095674480220028	1	0
101	2000000210	4768003828651437716	1	0
101	2000000211	8906314032266130370	1	4
101	2000000212	-7100702980283025836	1	2
101	2000000213	1344233066732476800	1	1
101	2000000214	7863800826459049259	1	2
101	2000000215	-5635970143258691064	1	0
101	2000000216	399603175506464863	1	1
101	2000000217	-4159700018826741568	1	0
101	2000000218	-1217956621720234165	1	0
101	2000000219	-4379048972556788231	1	4
101	2000000220	6910590723172147604	1	2
101	2000000221	-5436223351934266950	1	1
101	2000000222	3385906740431711903	1	0
101	2000000223	1015688014506287140	1	0
101	2000000224	-4545065987677879872	1	0
101	2000000225	-1690653735689708521	1	1
101	2000000226	9152719559837512697	1	2
101	2000000227	2112934594205575010	1	0
101	2000000228	-6921788027242763511	1	0
101	2000000229	9026668089164890926	1	0
101	2000000230	7835985019669643007	1	0
1	2000001363	5405163203147116641	1	5537
1	2000001364	8702385181061615546	1	9151
1	2000001365	8845825461403835908	1	2189
1	2000001366	3592681968420981920	1	529
1	2000001367	3740543862357774599	1	512
1	2000001368	6795260181976179461	1	703
101	2000000044	-5014417892241219914	1	0
101	2000000045	5317262931568349904	1	8
101	2000000046	982545823736371990	1	12
101	2000000047	-6375807517106078134	1	1
101	2000000048	3884006348627733517	1	2
101	2000000049	3773868157680640943	1	0
101	2000000050	4133740822589071545	1	5
101	2000000051	-631540076145704700	1	3
101	2000000052	471873135801994195	1	198
101	2000000053	-8211194052941157511	1	0
101	2000000054	4869087816672658090	1	12
101	2000000055	7307247278838092431	1	9
101	2000000056	-6754945327394567629	1	1
101	2000000057	-7530077675173574284	1	4
101	2000000058	-6813819657005622371	1	0
101	2000000059	5013891203746348224	1	0
101	2000000060	8139150691954259558	1	0
101	2000000061	-6863013941068350251	1	0
101	2000000062	8099482474164728756	1	3
101	2000000063	3502479247353265537	1	6
101	2000000064	4421890388133832713	1	1
101	2000000065	-1225547086238709623	1	3
101	2000000066	6004521876572272844	1	1
101	2000000067	-8579785066513323788	1	4
101	2000000068	5084515830392146249	1	8
101	2000000069	-7631616222065932609	1	37
101	2000000070	4054285518414936991	1	1
101	2000000071	855622501930378268	1	5
101	2000000072	4822464996449682968	1	1
101	2000000073	4804759450788955081	1	0
101	2000000074	2615522436833709908	1	0
101	2000000075	-5893936263433988855	1	0
101	2000000076	-2806122811888598155	1	0
101	2000000077	-323908055081131550	1	0
101	2000000078	-4228791055132281238	1	0
101	2000000079	4585975189901988855	1	0
101	2000000080	-1562883777249695314	1	0
101	2000000081	-2261324999257812422	1	0
101	2000000082	385756262405517557	1	0
101	2000000083	-1976503402141874080	1	0
101	2000000084	4525651972397617056	1	0
101	2000000085	828119402709425352	1	0
101	2000000086	5598680601370150278	1	0
101	2000000087	-3290616611505052031	1	25
101	2000000088	-3075720102990604265	1	1
101	2000000089	-4205356829618576577	1	0
101	2000000090	1541999056923217352	1	0
101	2000000091	-6729386337864249509	1	0
101	2000000092	-1636241169476163454	1	2
101	2000000093	-842028361149352176	1	0
101	2000000094	-5001296628537403263	1	0
101	2000000095	-4895682748520660873	1	405
101	2000000096	9207039181192489174	1	6
101	2000000097	265299714377165253	1	2
101	2000000098	1850931895478759887	1	48
101	2000000099	-3858185599271528368	1	14
101	2000000100	5369156447176937088	1	4
101	2000000101	2682672937639856679	1	3
101	2000000102	7644799529120265731	1	10
101	2000000103	546148726491252312	1	4
101	2000000104	-6925810533630939515	1	3
101	2000000105	2801260725862711702	1	3
101	2000000106	3985171750648239103	1	2
101	2000000107	-2094885574236634978	1	2
101	2000000108	-7293008259388994363	1	2
101	2000000109	8289558281556610212	1	21
101	2000000110	-8359869987379225846	1	7
101	2000000111	4643082334228461166	1	7
101	2000000112	-7182649009264205024	1	5
101	2000000113	-262372815853356321	1	5
101	2000000114	2853305180478063174	1	6
101	2000000115	273740604063040256	1	6
101	2000000116	5910254892637388891	1	68
101	2000000117	7296555989703615511	1	7
101	2000000118	-1647803935715495851	1	17
101	2000000119	-7216378367928879894	1	7
101	2000000120	2642972094844077548	1	12
101	2000000121	6061745905256248282	1	6
101	2000000122	4178047494179091499	1	17
101	2000000123	7899792671160188869	1	10
101	2000000124	-7946802209278770662	1	9
101	2000000125	-2095179500710035246	1	9
101	2000000126	-5370593418204654093	1	5
101	2000000127	5970637498973245149	1	5
101	2000000128	-5696787878964117539	1	6
101	2000000129	3886766662433091017	1	10
101	2000000130	8292494691086061859	1	9
101	2000000131	4985031533047976606	1	6
101	2000000132	8549850915877672668	1	9
101	2000000133	8229997000000531653	1	7
101	2000000134	-9128183645133246639	1	7
101	2000000135	7218161957043746000	1	6
101	2000000136	-7197118442584039090	1	16
101	2000000137	2498548561942185946	1	5
101	2000000138	8286629246610762820	1	4
101	2000000139	-1174808756205235933	1	6
101	2000000140	-8304354904086372845	1	5
101	2000000141	-901036977350955796	1	15
101	2000000231	1748930001085728374	1	0
101	2000000232	-4870757489143658629	1	0
101	2000000233	-8946682362426085462	1	0
101	2000000234	6482940291938471003	1	1
101	2000000235	-5100029622945035865	1	1
101	2000000236	5861494255440031174	1	1
101	2000000237	-3704034302743523421	1	0
101	2000000238	-3910353358171428391	1	0
101	2000000239	7178471542052155939	1	0
101	2000000240	-4846440185093465276	1	0
101	2000000241	-5098932985785461169	1	0
101	2000000242	3253163512775453786	1	0
101	2000000243	-2790709064176103988	1	2
101	2000000244	871906643331371361	1	0
101	2000000245	-4909190956136147169	1	0
101	2000000246	-811238283709010921	1	0
101	2000000247	-3044470657175993418	1	6
101	2000000248	7882824793751932003	1	1
101	2000000249	7742639209789501280	1	0
101	2000000250	-2046409902734848786	1	0
101	2000000251	-5682229402934137803	1	0
101	2000000252	-6243293462249496788	1	2
101	2000000253	8170965163082980576	1	0
101	2000000254	-1626235145649623620	1	0
101	2000000255	854370132355022024	1	0
101	2000000256	3273353310598347926	1	0
101	2000000257	8901378153430543944	1	1
101	2000000258	-6177029893441046382	1	0
101	2000000259	-7480954548352835387	1	0
101	2000000260	-3716360097816402166	1	0
101	2000000261	-1514631603694905049	1	0
101	2000000262	5140311844596731374	1	0
101	2000000263	671267312631720891	1	0
101	2000000264	-8631849075709006578	1	2
101	2000000265	3441965872625483974	1	0
101	2000000266	-5685808563646212420	1	0
101	2000000267	-5859128146916308836	1	0
101	2000000268	42973866600002177	1	1
101	2000000269	3442892229187295721	1	1
101	2000000270	-8635253897124206828	1	1
101	2000000271	5460041067622205790	1	1
101	2000000272	8068526363439912750	1	0
101	2000000273	-3276954727298532722	1	0
101	2000000274	-3844892461784491471	1	0
101	2000000275	8984070560824590680	1	0
101	2000000276	-6791269373554175731	1	1
101	2000000277	5413119706348108665	1	0
101	2000000278	-5649409396766215112	1	0
101	2000000279	-2956507839275432988	1	0
101	2000000280	6304524793301695953	1	0
101	2000000281	-2531294556859143024	1	0
101	2000000282	-3591361680031440861	1	1
101	2000000283	-4726818166408084722	1	0
101	2000000284	9160233208864691179	1	3
101	2000000285	8439687064434765458	1	0
101	2000000286	333483086096992788	1	0
101	2000000287	-2294112922519809294	1	2
101	2000000288	-1782386555252545714	1	0
101	2000000289	-2319932335302408233	1	1
101	2000000290	-6941344528592049593	1	0
101	2000000291	-8387929219434347608	1	0
101	2000000292	5534209550501235227	1	0
101	2000000293	-7842144647537605462	1	0
101	2000000294	3529563026656807306	1	0
101	2000000295	3292015599141719441	1	0
101	2000000296	-1956624619424199379	1	0
101	2000000297	2216113875507654384	1	0
101	2000000298	-6962178826515084706	1	27
101	2000000299	8840354425581028533	1	0
101	2000000300	-4659232289303006199	1	0
101	2000000301	2893223015212878224	1	16
101	2000000302	500549125022718037	1	11
101	2000000303	1596753244653843244	1	10
101	2000000304	2317779630936083971	1	8
101	2000000305	-4790867754608461845	1	8
101	2000000306	8355975946991213234	1	7
101	2000000307	4525469924466266750	1	78
101	2000000308	8339961779989768797	1	475
101	2000000309	6768425885042804800	1	450
101	2000000310	-3702124768431439213	1	5
101	2000000311	-2151332099485546499	1	1
101	2000000312	6805988862252946054	1	1
101	2000000313	4186957571204407323	1	1
101	2000000314	-1137552624966346092	1	0
101	2000000315	-6843401644120111781	1	1
101	2000000316	-4775450474354578645	1	0
101	2000000317	673073708176228709	1	1
101	2000000318	-6042258967121502890	1	1
101	2000000319	5454844165586025251	1	2
101	2000000320	4781022324913180018	1	191
101	2000000321	7198195242312020334	1	1
101	2000000322	8061551208747540058	1	0
101	2000000323	975291638248038318	1	1
101	2000000324	5296186274952516017	1	1
101	2000000325	-7220900720455481494	1	0
101	2000000326	4598769653297822368	1	0
101	2000000327	5557147014809399260	1	1
101	2000000328	-6300936569192378386	1	0
101	2000000329	6583713035894075762	1	1
101	2000000330	-7148742707063303148	1	4
101	2000000331	6891133056092466157	1	7
101	2000000332	8847504368228341853	1	4
101	2000000333	-769850871404066714	1	3
101	2000000334	8010487579726324572	1	4
101	2000000335	-1281815806852441397	1	4
101	2000000336	1938521020609095059	1	1
101	2000000337	3523884679671725957	1	2
101	2000000338	810398108306760348	1	12
101	2000000339	-3753456124963804734	1	10
101	2000000340	9055249480909265145	1	1
101	2000000341	1376848864724890411	1	3
101	2000000342	-7249976424530137073	1	2
101	2000000343	4964813406853494573	1	1
101	2000000344	-4591010665574936763	1	2
101	2000000345	287894396417678741	1	4
101	2000000346	-3260839014246629641	1	142
101	2000000347	4089557322633378077	1	5
101	2000000348	-37434540835908810	1	0
101	2000000349	-6012374541873459228	1	1
101	2000000350	-8900759627960524397	1	0
101	2000000351	-6743800908559635352	1	0
101	2000000352	8199428507212141948	1	0
101	2000000353	-7201485080435899140	1	0
101	2000000354	1921674646476416089	1	0
101	2000000355	-1963231258293348397	1	0
101	2000000356	-8218325220138309565	1	0
101	2000000357	-5848613183465042385	1	0
101	2000000358	7878477018103303313	1	0
101	2000000359	-652171486573031821	1	0
101	2000000360	-6985289698676835606	1	0
101	2000000361	-5041753597538503662	1	0
101	2000000362	3530066322638971749	1	0
101	2000000363	-2370561505487284775	1	0
101	2000000364	1672842826120172223	1	0
101	2000000365	-2389231846021142751	1	0
101	2000000366	-5707071873651388052	1	0
101	2000000367	5504638158992425192	1	0
101	2000000368	-3103166860339059077	1	0
101	2000000369	-8762091848778466957	1	0
101	2000000370	-2138427480696216105	1	0
101	2000000371	-8727409099088109660	1	1
101	2000000372	-5455537310022155887	1	1
101	2000000373	-167802844271584258	1	0
101	2000000374	2198556615269313949	1	0
101	2000000375	7040673462681810840	1	0
101	2000000376	123339232929051273	1	0
101	2000000377	-3971936715945561297	1	0
101	2000000378	-5889541757896086778	1	0
101	2000000379	7380966007935825479	1	0
101	2000000380	5345430275619616017	1	0
101	2000000381	-3268371542770224896	1	0
101	2000000382	-9150411862071801368	1	0
101	2000000383	-8627233018830102918	1	0
101	2000000384	2383175652925770064	1	0
101	2000000385	-3529455259661086276	1	0
101	2000000386	8112924697463551049	1	0
101	2000000387	-3889480022973573131	1	11
101	2000000388	-554800153383183542	1	48
101	2000000389	3174605870365863445	1	8
101	2000000390	-6010461173274098506	1	4
101	2000000391	3972174041477632771	1	10
101	2000000392	-599618953858237035	1	1
101	2000000393	-3332655978695007023	1	5
101	2000000394	3334900419860551377	1	2
101	2000000395	-8391786289264016656	1	2
101	2000000396	2324740851889752724	1	18
101	2000000397	-1306535233403231905	1	5
101	2000000398	3691910877796480479	1	6
101	2000000399	4101574638442679856	1	5
101	2000000400	2528460505267493704	1	1
101	2000000401	3868823698102365907	1	33
101	2000000402	1210847126746082376	1	2
101	2000000403	6274690045188883515	1	3
101	2000000404	2448275589862489061	1	2
101	2000000405	1019708284290908756	1	1
101	2000000406	5731224940909260024	1	1
101	2000000407	-4461578282072854891	1	1
101	2000000408	4422570313003487721	1	1
101	2000000409	-6975057738140130921	1	1
101	2000000410	2583041290795623063	1	1
101	2000000411	4554372258448489687	1	1
101	2000000412	4676693325818911216	1	0
101	2000000413	-6899431719471549278	1	5
101	2000000414	3509701785538232647	1	1
101	2000000415	-9072473139632003956	1	5
101	2000000416	-8218772127320035485	1	0
101	2000000417	-1340594121368301723	1	3
101	2000000418	-6617398307302016737	1	3
101	2000000419	7377611215149686245	1	5
101	2000000420	-8717890372998608465	1	2
101	2000000421	-3768139355512319682	1	2
101	2000000422	186989653214630812	1	2
101	2000000423	-8575991715294204431	1	2
101	2000000424	-2135398279124016704	1	1
101	2000000425	3745991302157209440	1	0
101	2000000426	7200778840955335664	1	0
101	2000000427	5403114043803834687	1	1
101	2000000428	7471078443531567468	1	0
101	2000000429	-6516337435024933937	1	0
101	2000000430	-8157418118392356068	1	0
101	2000000431	-8941669507633053478	1	1
101	2000000432	1750220706064829111	1	0
101	2000000433	-323380473992954954	1	0
101	2000000434	-4750834435677326447	1	0
101	2000000435	6395554917994421796	1	3
101	2000000436	-2393752537121269767	1	1
101	2000000437	-6232339777137704434	1	0
101	2000000438	-520295747944787748	1	0
101	2000000439	8935027621324685126	1	1
101	2000000440	1819288246643318899	1	1
101	2000000441	9082917390085156165	1	0
101	2000000442	2677094157800687234	1	0
101	2000000443	-5118235684681422184	1	0
101	2000000444	-7672359881715126002	1	0
101	2000000445	-3274095225854377694	1	3
101	2000000446	4773013725081228387	1	0
101	2000000447	8295892158328255943	1	0
101	2000000448	5946293114760361894	1	2
101	2000000449	-2857515491886374742	1	3
101	2000000450	-9016000243843539149	1	0
101	2000000451	7991943861722550330	1	1
101	2000000452	-5032889823836522363	1	1
101	2000000453	-1069215540778290849	1	0
101	2000000454	7933044701474115079	1	0
101	2000000455	3383483138513852096	1	0
101	2000000456	-5021294807930804702	1	0
101	2000000457	3232398040969668304	1	1
101	2000000458	5701699701766514880	1	2
101	2000000459	1079777846125395147	1	3
101	2000000460	-4757185636571062174	1	1
101	2000000461	631308306954226985	1	160
101	2000000462	6592849061380933631	1	1
101	2000000463	100475979119385272	1	0
101	2000000464	4488047058959874712	1	2
101	2000000465	6235711471212629325	1	0
101	2000000466	3450623954380032235	1	0
101	2000000467	-7805929135540215937	1	0
101	2000000468	-2912815792102701509	1	0
101	2000000469	-3149577814460868489	1	1
101	2000000470	2419264815177948334	1	0
101	2000000471	2892885172302753480	1	0
101	2000000472	2609711005454547867	1	0
101	2000000473	-517343203919092306	1	0
101	2000000474	-9124560809556394670	1	0
101	2000000475	-8330373889145914487	1	0
101	2000000476	8747320628437539152	1	0
101	2000000477	-6945858700441151447	1	0
101	2000000478	-6081354969982681999	1	0
101	2000000479	4035584893480932499	1	0
101	2000000480	-7056374640302649604	1	0
101	2000000481	-4791221260792473098	1	0
101	2000000482	-5333581857906755938	1	0
101	2000000483	657025011518556863	1	0
101	2000000484	-8970379998755300367	1	0
101	2000000485	-7040402825184554156	1	0
101	2000000486	-1460346436665267622	1	0
101	2000000487	-9022769976017791560	1	1
101	2000000488	8308133133012897805	1	0
101	2000000489	-5498163394167153218	1	0
101	2000000490	5204670132024648480	1	1
101	2000000491	-5006680664044913737	1	0
101	2000000492	8218793679501148847	1	0
101	2000000493	-3184534676668196556	1	0
101	2000000494	-1507875266406929359	1	0
101	2000000495	6412966820676540751	1	0
101	2000000496	5746705507760678982	1	0
101	2000000497	-1400974169623107081	1	0
101	2000000498	6386329771030373937	1	0
101	2000000499	1929673210871152925	1	0
101	2000000500	302047598451694000	1	14
101	2000000501	2062383995271269786	1	0
101	2000000502	-3325137484275582249	1	0
101	2000000503	3720236562660988131	1	0
101	2000000504	5455646959622234629	1	0
101	2000000505	-8946073386512960103	1	0
101	2000000506	-6933499149927628326	1	0
101	2000000507	3619311731473916220	1	0
101	2000000508	-3540429236196209402	1	0
101	2000000509	1680103686424152264	1	1
101	2000000510	5550703416682629513	1	0
101	2000000511	-1261190560869627214	1	0
101	2000000512	-7382620743575113589	1	0
101	2000000513	-6481782723578544090	1	0
101	2000000514	-6271626231804380759	1	0
101	2000000515	-2362168989856320897	1	0
101	2000000516	-3747520104157667041	1	0
101	2000000517	-6274934036837929527	1	0
101	2000000518	9063175315430654317	1	0
101	2000000519	3600368723188857852	1	0
101	2000000520	-3275278053552906791	1	0
101	2000000521	-4089443421389216307	1	0
101	2000000522	2209504073079976925	1	0
101	2000000523	6742207834329338799	1	0
101	2000000524	-1931136746910890635	1	0
101	2000000525	-8923877033546041150	1	2
101	2000000526	4361595946815463385	1	1
101	2000000527	-7126657288987151105	1	3
101	2000000528	-8474275296072444440	1	0
101	2000000529	-829046258088547010	1	0
101	2000000530	-3429772387199373398	1	0
101	2000000531	4287622489691514106	1	0
101	2000000532	4305927349006483485	1	1
101	2000000533	-5891975601941594263	1	0
101	2000000534	3552664310779555538	1	1
101	2000000535	77098920388313739	1	0
101	2000000536	1008163528827344863	1	0
101	2000000537	2378735368864199773	1	0
101	2000000538	-411873028061658547	1	1
101	2000000539	-4181691547645504898	1	1
101	2000000540	375420428497839569	1	0
101	2000000541	-4814854482884217950	1	0
101	2000000542	-5052095623130965223	1	0
101	2000000543	8665316472139466444	1	0
101	2000000544	-2352759059289503719	1	0
101	2000000545	-9185417756159682726	1	0
101	2000000546	-2537876146835428001	1	0
101	2000000547	1988307758015769345	1	0
101	2000000548	7375152660932761054	1	0
101	2000000549	-2204572513767827810	1	0
101	2000000550	-4083001903083056848	1	0
101	2000000551	-6417616407480563538	1	0
101	2000000552	-1090872573128234889	1	2
101	2000000553	-1870455361179779121	1	0
101	2000000554	-3911209253402778809	1	0
101	2000000555	1819032009935960307	1	0
101	2000000556	-7772786161986220840	1	0
101	2000000557	2887177096307434411	1	0
101	2000000558	-8330862128140684673	1	0
101	2000000559	-3752822565344380246	1	0
101	2000000560	-2642196237391216400	1	0
101	2000000561	-7514727290563466446	1	0
101	2000000562	-8803297981895917015	1	0
101	2000000563	1377339183922792491	1	0
101	2000000564	4139768908245332268	1	1
101	2000000565	-7258685998976334159	1	0
101	2000000566	8476909923968604590	1	0
101	2000000567	-6157168172706026864	1	0
101	2000000568	-3602472103318952577	1	0
101	2000000569	-6377809419111902802	1	0
101	2000000570	5629305678908643644	1	1
101	2000000571	1466672490978449442	1	0
101	2000000572	3936942997757393197	1	1
101	2000000573	-4021020179368974406	1	1
101	2000000574	-6841055803091445449	1	0
101	2000000575	3254722748319759749	1	0
101	2000000576	8770879149278552638	1	0
101	2000000577	-547175503758867039	1	18
101	2000000578	8267360228842387378	1	12
101	2000000579	-8840173634241471262	1	1
101	2000000580	-5353300839657544953	1	0
101	2000000581	-674673763740317037	1	0
101	2000000582	-316447274411310824	1	0
101	2000000583	-1591481693817104529	1	0
101	2000000584	2223566409522421057	1	0
101	2000000585	-1899365803510401341	1	0
101	2000000586	-1203559981284246877	1	0
101	2000000587	5783387881951179883	1	0
101	2000000588	8035979234755379503	1	0
101	2000000589	158014887412681138	1	0
101	2000000590	108268604432865115	1	0
101	2000000591	4027233132685793832	1	0
101	2000000592	-8836624144465935612	1	0
101	2000000593	224841885053618169	1	0
101	2000000594	-473344733180064987	1	1
101	2000000595	7303097711185187609	1	0
101	2000000596	6410203933308010749	1	0
101	2000000597	7144604583276889671	1	0
101	2000000598	-923832771737137593	1	0
101	2000000599	472257259985170296	1	0
101	2000000600	-7183873401417815368	1	0
101	2000000601	5639561393726250596	1	0
101	2000000602	-748429510391056660	1	0
101	2000000603	-1471821691389305120	1	0
101	2000000604	-1244110861695361205	1	0
101	2000000605	1849356259714524830	1	0
101	2000000606	5372648116563306155	1	0
101	2000000607	3788322937277224048	1	0
101	2000000608	1741724177889364421	1	0
101	2000000609	-1544374647241629962	1	0
101	2000000610	2270205132328204056	1	0
101	2000000611	4570899514858431606	1	0
101	2000000612	-1151592955510978112	1	0
101	2000000613	3641081747835851970	1	0
101	2000000614	3618095136331371696	1	0
101	2000000615	-6147070407927747851	1	0
101	2000000616	-8482464526912559302	1	0
101	2000000617	1454769167281619589	1	0
101	2000000618	-7160657270513693954	1	0
101	2000000619	-5707501221620982254	1	0
101	2000000620	-3393924028984365802	1	0
101	2000000621	218113030405659023	1	0
101	2000000622	-7326307373869845509	1	2
101	2000000623	8994507315322889238	1	0
101	2000000624	7113924106942583828	1	0
101	2000000625	-1178101138277602910	1	0
101	2000000626	6683456173721167212	1	0
101	2000000627	-5187881189794042804	1	0
101	2000000628	-5866543025278487313	1	1
101	2000000629	-1733326771329823463	1	1
101	2000000630	-7883477971508375745	1	0
101	2000000631	7861568514571853282	1	1
101	2000000632	3976811048074606695	1	1
101	2000000633	-7657782823826474449	1	1
101	2000000634	8033395148388329747	1	1
101	2000000635	7521579517602025726	1	0
101	2000000636	5133758865275727318	1	0
101	2000000637	353410713350147686	1	0
101	2000000638	4370706321304864293	1	0
101	2000000639	-604599232154294899	1	0
101	2000000640	4200379726516780142	1	0
101	2000000641	-4856317104683875529	1	0
101	2000000642	2948843064339980442	1	0
101	2000000643	5226501621194592299	1	0
101	2000000644	-3749193557444209145	1	0
101	2000000645	-3068262110343687907	1	0
101	2000000646	-7371290191581905607	1	0
101	2000000647	-1497134584702649418	1	0
101	2000000648	4836779721170942861	1	0
101	2000000649	2330984446180266726	1	0
101	2000000650	-4941497842926012142	1	1
101	2000000651	3850299087064257896	1	1
101	2000000652	1379885231265209318	1	0
101	2000000653	4730900279908313531	1	0
101	2000000654	-6692942897719607452	1	0
101	2000000655	6596385218658280824	1	0
101	2000000656	7742280597781519260	1	0
101	2000000657	-4939379533109856050	1	0
101	2000000658	-559271542322089807	1	0
101	2000000659	-4988170037811856119	1	0
101	2000000660	-1642963215527835214	1	0
101	2000000661	160470546533425842	1	0
101	2000000662	-4597536811667411047	1	0
101	2000000663	-5225835116227179315	1	0
101	2000000664	2012674202134181162	1	0
101	2000000665	9219722906344172547	1	0
101	2000000666	3789344299255261546	1	0
101	2000000667	-1110379169338656866	1	0
101	2000000668	-3259485908059675943	1	0
101	2000000669	7433331124027422644	1	1
101	2000000670	7999208031922245120	1	1
101	2000000671	5805668549733399878	1	1
101	2000000672	3314571711313360320	1	0
101	2000000673	-3548259621211206315	1	0
101	2000000674	4746829911096675271	1	0
101	2000000675	-7623144499908919361	1	0
101	2000000676	-6723657192789197229	1	0
101	2000000677	-6335971593227921026	1	0
101	2000000678	-5775568776361052001	1	0
101	2000000679	-8070008019278497384	1	0
101	2000000680	5679072309791352125	1	0
101	2000000681	8876830551412023741	1	0
101	2000000682	-3460807262607593561	1	0
101	2000000683	3293389237072111462	1	0
101	2000000684	-6436209041997647296	1	0
101	2000000685	6548174634444797682	1	0
101	2000000686	-8522120391317385162	1	0
101	2000000687	777704923058060297	1	0
101	2000000688	-900840466130694104	1	0
101	2000000689	4832504558073963968	1	0
101	2000000690	-8381922247839435305	1	0
101	2000000691	-704062481181072710	1	0
101	2000000692	2020796491414859841	1	0
101	2000000693	-2290156110695935850	1	0
101	2000000694	2302167374604391005	1	0
101	2000000695	-5674698766906137194	1	0
101	2000000696	-9183290367865991264	1	0
101	2000000697	7498069002045135382	1	0
101	2000000698	8894193892046569413	1	0
101	2000000699	-7704236595691425209	1	0
101	2000000700	7579344795684748176	1	0
101	2000000701	6996668969732783144	1	0
101	2000000702	-8009200561564919891	1	0
101	2000000703	5603297846530487325	1	0
101	2000000704	3274810119064705259	1	0
101	2000000705	3146147149573717354	1	6
101	2000000706	-2228400562854313153	1	0
101	2000000707	-8781065821573446522	1	0
101	2000000708	-6650893164806474862	1	0
101	2000000709	-3532027586826254054	1	0
101	2000000710	-7076876310571766028	1	1
101	2000000711	-7871678614102366785	1	0
101	2000000712	-947369421393944280	1	0
101	2000000713	-7249504719250979845	1	0
101	2000000714	5163656879946869082	1	1
101	2000000715	-6689405592075286178	1	0
101	2000000716	2041071870319067295	1	0
101	2000000717	7315194354421844525	1	0
101	2000000718	-1981447961786148634	1	0
101	2000000719	-7761558375622485442	1	1
101	2000000720	2847239473139498991	1	0
101	2000000721	8511227886358174355	1	0
101	2000000722	8663491027335178694	1	0
101	2000000723	-7082171019313824030	1	0
101	2000000724	-6964947218939580574	1	0
101	2000000725	1566535363509039560	1	1
101	2000000726	-5864548989101485226	1	1
101	2000000727	-8884838108438765824	1	0
101	2000000728	-6892007837533608991	1	5
101	2000000729	6640252121598649676	1	0
101	2000000730	6974283897199924901	1	0
101	2000000731	-2413253437294506737	1	0
101	2000000732	-5745046589739276255	1	0
101	2000000733	-2816095407172438235	1	0
101	2000000734	-1526317184543969641	1	0
101	2000000735	-4236279152836641538	1	0
101	2000000736	5883450169777154787	1	0
101	2000000737	-8885949884843028693	1	0
101	2000000738	-4275480352121319611	1	0
101	2000000739	4464399667694645336	1	0
101	2000000740	-8230243735332585566	1	0
101	2000000741	3843506714796885343	1	0
101	2000000742	-6438327568176191801	1	0
101	2000000743	-936893693115527437	1	0
101	2000000744	-8722452410758653635	1	0
101	2000000745	9175510766633525267	1	0
101	2000000746	407113350092855760	1	0
101	2000000747	190569239989277010	1	0
101	2000000748	-1675329261346332849	1	0
101	2000000749	-6997337568353676874	1	0
101	2000000750	5400288476079911027	1	5
101	2000000751	-5094202754092917760	1	1
101	2000000752	-6281351739100713367	1	1
101	2000000753	-8356487029510250283	1	0
101	2000000754	-2768266330783356096	1	0
101	2000000755	2991194027017732164	1	0
101	2000000756	6817250711474100479	1	0
101	2000000757	-8609459946517706479	1	0
101	2000000758	1117597016868852500	1	1
101	2000000759	5480063571633879688	1	0
101	2000000760	-786012976648189977	1	0
101	2000000761	-7867237249420254429	1	0
101	2000000762	78506733819387106	1	0
101	2000000763	7343414712412368710	1	0
101	2000000764	-3616245965538611598	1	28
101	2000000765	9062243209505088646	1	0
101	2000000766	-5589140067204984410	1	0
101	2000000767	-4620830751155899334	1	0
101	2000000768	-4685899739859365940	1	0
101	2000000769	5988353012999221631	1	0
101	2000000770	1625551037688455344	1	0
101	2000000771	4612079968168220183	1	0
101	2000000772	-8903470396850240656	1	0
101	2000000773	7471397317073184649	1	0
101	2000000774	-9073260828057306664	1	0
101	2000000775	-6004689372673379646	1	0
101	2000000776	-638364817117994440	1	1
101	2000000777	-2322817597090801987	1	1
101	2000000778	-6912443921216102974	1	0
101	2000000779	-5206855240369371018	1	0
101	2000000780	3301255963549947634	1	0
101	2000000781	4215354479734997830	1	1
101	2000000782	6581433749055524307	1	0
101	2000000783	5546863820523493479	1	0
101	2000000784	255751341649350614	1	0
101	2000000785	-6627260910814133887	1	0
101	2000000786	1451008053836700258	1	0
101	2000000787	5912273332061696898	1	0
101	2000000788	4914174051134168774	1	1
101	2000000789	-1505864365404196627	1	0
101	2000000790	-727451794362875038	1	0
101	2000000791	2894744210331295187	1	0
101	2000000792	1772480079821854380	1	0
101	2000000793	6535182933820282663	1	0
101	2000000794	5501484454222036205	1	0
101	2000000795	665505556097720381	1	0
101	2000000796	5086047834088109126	1	0
101	2000000797	-1140359889630538607	1	0
101	2000000798	7731012049117049076	1	0
101	2000000799	4038449796716148440	1	0
101	2000000800	-2664297099276952766	1	2
101	2000000801	-2482500879906466549	1	1
101	2000000802	2425597280237009217	1	1
101	2000000803	-586652006490389178	1	0
101	2000000804	6580682854756946552	1	0
101	2000000805	686799993030402996	1	1
101	2000000806	4874361422224701319	1	0
101	2000000807	8502745391283177618	1	0
101	2000000808	8614401276580995037	1	0
101	2000000809	-1327063142942510431	1	0
101	2000000810	1932581422101276016	1	0
101	2000000811	-2291857299264558162	1	0
101	2000000812	-4080222699972974441	1	1
101	2000000813	7720997577829840202	1	0
101	2000000814	3408792733589247099	1	0
101	2000000815	8691023200992283088	1	0
101	2000000816	-242433313724515810	1	1
101	2000000817	2673070882760588525	1	27
101	2000000818	3049192474232067376	1	0
101	2000000819	-2575370679915686837	1	1
101	2000000820	-3076343116555089877	1	0
101	2000000821	5287458118285883886	1	0
101	2000000822	694918346179025770	1	0
101	2000000823	-6342531851170206187	1	0
101	2000000824	-1692105356781825231	1	0
101	2000000825	-992377258616668112	1	0
101	2000000826	-6311014117888634806	1	0
101	2000000827	4848000494819026557	1	0
101	2000000828	1059691320582458907	1	0
101	2000000829	-3032344655395207967	1	0
101	2000000830	-2372598623342294744	1	0
101	2000000831	5425160292901546755	1	0
101	2000000832	76488856091185374	1	1
101	2000000833	1224547687312934289	1	0
101	2000000834	-4155611304333476112	1	0
101	2000000835	-5570697310300715893	1	0
101	2000000836	-4576905349066182435	1	0
101	2000000837	7610556504345267071	1	0
101	2000000838	-7470257043996736661	1	0
101	2000000839	416687423359059407	1	0
101	2000000840	7228942233047580204	1	0
101	2000000841	4546933048618210470	1	0
101	2000000842	-3440651093663044021	1	1
101	2000000843	-292277682007308740	1	0
101	2000000844	4324309647875998771	1	0
101	2000000845	-1178792961506915486	1	0
101	2000000846	-1452112783999086304	1	0
101	2000000847	4208383395991285259	1	2
101	2000000848	2681240363186309918	1	0
101	2000000849	5948516854699542069	1	0
101	2000000850	-8282321649916488413	1	1
101	2000000851	-7870819632915365856	1	5
101	2000000852	7436420335013592145	1	1
101	2000000853	-2047916398335965857	1	0
101	2000000854	-9015728697059075019	1	0
101	2000000855	728247740357176199	1	0
101	2000000856	-9165043103091401601	1	0
101	2000000857	-3020579081032030055	1	0
101	2000000858	4413574436139789906	1	0
101	2000000859	8388470501585506122	1	2
101	2000000860	-4875998285682959509	1	0
101	2000000861	-8011489024666563166	1	0
101	2000000862	4308079095349473247	1	0
101	2000000863	1091469637333404721	1	0
101	2000000864	-4473303655182815444	1	0
101	2000000865	-2275697846518862928	1	3
101	2000000866	-9184799163675668790	1	0
101	2000000867	4057932499337166717	1	0
101	2000000868	-6006606298310346554	1	0
101	2000000869	-7255101302504907991	1	0
101	2000000870	4712272820753174645	1	1
101	2000000871	-7673554198257650506	1	0
101	2000000872	-7262478061143821468	1	0
101	2000000873	5858929025123377530	1	0
101	2000000874	-8351617673807613926	1	0
101	2000000875	-6339998741219588628	1	0
101	2000000876	-1930517508288140553	1	0
101	2000000877	-5004865016691376838	1	0
101	2000000878	-1229892140439693211	1	1
101	2000000879	-7521650831978226364	1	1
101	2000000880	4176625386367073818	1	1
101	2000000881	4794068930792434294	1	0
101	2000000882	-4988149378914569423	1	0
101	2000000883	-6860337138445249149	1	0
101	2000000884	3964857468385941503	1	0
101	2000000885	-4989319812405585052	1	0
101	2000000886	1742266396999405263	1	0
101	2000000887	3886518409372390089	1	0
101	2000000888	-1135443937584918265	1	1
101	2000000889	2006855055502907474	1	0
101	2000000890	-768227824610522298	1	0
101	2000000891	4716354392741012469	1	0
101	2000000892	-3666150636673913382	1	1
101	2000000893	5742032320271158230	1	0
101	2000000894	8039513648741184249	1	0
101	2000000895	-4757149717594373648	1	0
101	2000000896	912800248653254451	1	0
101	2000000897	2171890429284314195	1	0
101	2000000898	6104628488879558047	1	1
101	2000000899	-3467873180011179379	1	0
101	2000000900	-5525605133353891528	1	0
101	2000000901	-6346860121539058337	1	0
101	2000000902	4814594667029517543	1	0
101	2000000903	544509640253020970	1	1
101	2000000904	265829263482519944	1	0
101	2000000905	44681976019897786	1	0
101	2000000906	-5744614909404880627	1	0
101	2000000907	5835430415156872082	1	0
101	2000000908	3465093300269864384	1	1
101	2000000909	-3078742353904403650	1	0
101	2000000910	882326949164834635	1	0
101	2000000911	7664680734285423530	1	0
101	2000000912	883003652861180376	1	1
101	2000000913	-7293576720655050036	1	1
101	2000000914	7923350681241079024	1	1
101	2000000915	-2056343294866171643	1	1
101	2000000916	9081816143670670034	1	0
101	2000000917	-967846679142506426	1	0
101	2000000918	-7204819334085466013	1	0
101	2000000919	5880049292494101918	1	4
101	2000000920	8221084992272232112	1	2
101	2000000921	609604938207610433	1	0
101	2000000922	-347706343151557748	1	1
101	2000000923	-4569641039552417056	1	1
101	2000000924	-4976273116129123078	1	0
101	2000000925	-2625437675942997275	1	0
101	2000000926	7729616558499493031	1	0
101	2000000927	3836076711622384050	1	0
101	2000000928	-8805606788410929812	1	0
101	2000000929	63851718022001911	1	0
101	2000000930	9205696324497013	1	0
101	2000000931	8724533709734202211	1	0
101	2000000932	256728256771641962	1	0
101	2000000933	-1131743231451411598	1	0
101	2000000934	-2449078386832694601	1	0
101	2000000935	6064964023703205388	1	1
101	2000000936	8966706877660743097	1	0
101	2000000937	-4025443318567146955	1	0
101	2000000938	5485868942381942023	1	0
101	2000000939	5879881600104610197	1	0
101	2000000940	-2939408616828985634	1	0
101	2000000941	5406123222108191294	1	0
101	2000000942	4343929996627703905	1	0
101	2000000943	-7672027769739477145	1	0
101	2000000944	7437103956357837389	1	0
101	2000000945	7915545078928575986	1	0
101	2000000946	-8545790135179170598	1	0
101	2000000947	701031601927376825	1	0
101	2000000948	5888195715833119222	1	0
101	2000000949	-9112096851723170186	1	0
101	2000000950	3384374749149532045	1	0
101	2000000951	-327349863373322152	1	0
101	2000000952	-6813084329999632373	1	0
101	2000000953	-7917844593539014293	1	0
101	2000000954	-3560994649842946011	1	0
101	2000000955	2367462802463363422	1	0
101	2000000956	-3613740110692547712	1	0
101	2000000957	6589356139030126645	1	0
101	2000000958	3528086416049257714	1	2
101	2000000959	7071521955825074980	1	0
101	2000000960	4670464998368903761	1	1
101	2000000961	8052128727758468466	1	0
101	2000000962	-4027489574189355398	1	0
101	2000000963	-7772401465520405793	1	0
101	2000000964	-7864230203590923057	1	1
101	2000000965	8910539476105716733	1	0
101	2000000966	-4928145995967954691	1	0
101	2000000967	-986376273584432818	1	1
101	2000000968	-4640904247616785874	1	0
101	2000000969	8190960998390422427	1	0
101	2000000970	-4608901660714913337	1	0
101	2000000971	1275011559545855911	1	0
101	2000000972	-1295201336329561053	1	0
101	2000000973	9091829820384950553	1	2
101	2000000974	6540957371066614316	1	0
101	2000000975	-2019419135082391874	1	0
101	2000000976	-2398904949545213388	1	0
101	2000000977	-9190790590285646942	1	0
101	2000000978	7849459939427238065	1	0
101	2000000979	-8299700317371339995	1	0
101	2000000980	-377606298713362625	1	0
101	2000000981	-2031725293327449026	1	1
101	2000000982	5983264011791497714	1	1
101	2000000983	-7690610360618111555	1	0
101	2000000984	449937141918057277	1	0
101	2000000985	6914732839229375633	1	0
101	2000000986	-8850891524784861637	1	0
101	2000000987	8533968584872633970	1	1
101	2000000988	681146999070498170	1	1
101	2000000989	-3450014328039069239	1	0
101	2000000990	-8342820603299250897	1	0
101	2000000991	-1091438705083176208	1	0
101	2000000992	5902272033214131380	1	1
101	2000000993	7908348201613442623	1	0
101	2000000994	-5149731834386805752	1	0
101	2000000995	-1512542050362921247	1	0
101	2000000996	-2555596903551353834	1	0
101	2000000997	-8591353267454696957	1	0
101	2000000998	4648438223477796593	1	2
101	2000000999	2245323675258877451	1	0
101	2000001000	6959475857629491259	1	1
101	2000001001	-7125449764991703689	1	57
101	2000001002	-2806368011398906520	1	0
101	2000001003	5594314531942316630	1	1
101	2000001004	-8988012340689947584	1	1
101	2000001005	-4528637836425608369	1	1
101	2000001006	-1135189464947323845	1	1
101	2000001007	2885526710536598113	1	2
101	2000001008	1286238108062068368	1	0
101	2000001009	-6648944238338842481	1	1
101	2000001010	6860362571554215216	1	1
101	2000001011	321554501983926968	1	0
101	2000001012	5367931265214041181	1	1
101	2000001013	3538872270517270634	1	0
101	2000001014	7732139213757531631	1	0
101	2000001015	6714215559856081395	1	1
101	2000001016	-7641392127491465904	1	0
101	2000001017	2276997983505728369	1	1
101	2000001018	-1007467999953333188	1	2
101	2000001019	2374965325163934168	1	2
101	2000001020	5616213889778005441	1	5
101	2000001021	8455353873766678633	1	4
101	2000001022	-8398102793415941379	1	4
101	2000001023	-2780071311272975110	1	2
101	2000001024	-8053000820966812681	1	5
101	2000001025	-6664754518424090642	1	8
101	2000001026	-6570008418861600440	1	3
101	2000001027	6551778862476105365	1	24
101	2000001028	6627834638240895512	1	16
101	2000001029	8315107481872866927	1	26
101	2000001030	5670771697453772560	1	1
101	2000001031	1544155196440893856	1	0
101	2000001032	-4770951617551334686	1	2
101	2000001033	-372345773148852929	1	0
101	2000001034	-5733967213181359467	1	2
101	2000001035	-4176918439774775822	1	3
101	2000001036	5691238000848924670	1	1
101	2000001037	-3085202136643791384	1	14
101	2000001038	-6131068197541576738	1	3
101	2000001039	-6862115297513520503	1	2
101	2000001040	-464283600867059024	1	6
101	2000001041	7186828315059264889	1	11
101	2000001042	6158124222323029640	1	3
101	2000001043	-8464602885554311459	1	3
101	2000001044	108747157671611731	1	1
101	2000001045	8462740860802420442	1	1
101	2000001046	-2399654172080959678	1	19
101	2000001047	-8910952457536193267	1	0
101	2000001048	-6906691816955658071	1	1
101	2000001049	-3635710217030240506	1	2
101	2000001050	5857327834671419286	1	10
101	2000001051	-5928123676542923322	1	0
101	2000001052	-1222368133678978019	1	0
101	2000001053	5269657952108192472	1	1
101	2000001054	2067208644914676018	1	14
101	2000001055	-2638291134221394996	1	20
101	2000001056	2153771988474078008	1	8
101	2000001057	-960128883284789234	1	3
101	2000001058	2460692043327998509	1	6
101	2000001059	-367814829649005664	1	15
101	2000001060	1237306865968645953	1	9
101	2000001061	-768449131088247831	1	18
101	2000001062	-891949923407065854	1	6
101	2000001063	-7458424375290572939	1	6
101	2000001064	5898754603752214121	1	4
101	2000001065	6447188493202147399	1	13
101	2000001066	8522311862062046826	1	10
101	2000001067	3009239510960794656	1	11
101	2000001068	4233681671533271443	1	46
101	2000001069	-288608372800445033	1	7
101	2000001070	-1688916975541725591	1	9
101	2000001071	4230669388504180511	1	3
101	2000001072	-1441439608458833047	1	1
101	2000001073	-8693463573350018352	1	0
101	2000001074	-6822581025649830660	1	0
101	2000001075	5299779084982604411	1	17
101	2000001076	7341252202662817654	1	3
101	2000001077	-3734011219728813011	1	2
101	2000001078	-3111047399857813227	1	16
101	2000001079	6356044858478115513	1	5
101	2000001080	7631961285996600547	1	4
101	2000001081	4078814598570768663	1	18
101	2000001082	-2671902171571531285	1	4
101	2000001083	5121651198702756892	1	5
101	2000001084	87388039916460421	1	9
101	2000001085	911759618204730973	1	3
101	2000001086	4291486113038902560	1	5
101	2000001087	-6881348954402300356	1	4
101	2000001088	-6709824203167960709	1	5
101	2000001089	-8128155084156210464	1	5
101	2000001090	-2453704373727120087	1	4
101	2000001091	8257310182211849555	1	5
101	2000001092	2005531911630964251	1	7
101	2000001093	-8131967445052921272	1	4
101	2000001094	-52907678272727815	1	4
101	2000001095	7116507851557313248	1	4
101	2000001096	-7215120721757903283	1	3
101	2000001097	-8960805452828810794	1	4
101	2000001098	-7067785902223299339	1	4
101	2000001099	-1763050638754675372	1	0
101	2000001100	5780901546479079195	1	3
101	2000001101	5524856594142199329	1	1
101	2000001102	3863217587438177010	1	1
101	2000001103	-6530690482946375377	1	2
101	2000001104	5437554715955851851	1	2
101	2000001105	3211889679154096856	1	8
101	2000001106	-2395467479759687223	1	0
101	2000001107	-2573991658597989803	1	0
101	2000001108	-4377735459434946599	1	1
101	2000001109	-72027690974534248	1	0
101	2000001110	207189933907901365	1	0
101	2000001111	2361921548870230688	1	0
101	2000001112	-3252907002280195029	1	7
101	2000001113	4604669814823382222	1	0
101	2000001114	-6435126593051665603	1	6
101	2000001115	7282032306123914706	1	9
101	2000001116	848326468934320527	1	5
101	2000001117	9190602656340832148	1	2
101	2000001118	-6473671706617561918	1	6
101	2000001119	-5726951638775068934	1	1
101	2000001120	-9079328690913641210	1	0
101	2000001121	-9210625787600764149	1	0
101	2000001122	4513977049716890332	1	4
101	2000001123	-9158713616993224031	1	3
101	2000001124	4374512322371477589	1	9
101	2000001125	6357542954624213120	1	3
101	2000001126	7547618722826960891	1	9
101	2000001127	-8143904735275224676	1	3
101	2000001128	-1306018922829213234	1	1
101	2000001129	3842414158465501632	1	3
101	2000001130	3670611775151130911	1	23
101	2000001131	2029903199701682342	1	3
101	2000001132	5516171768802322427	1	0
101	2000001133	8345614292201443968	1	0
101	2000001134	-8879581351573486110	1	0
101	2000001135	8605820832777887285	1	1
101	2000001136	-3966573541100699617	1	1
101	2000001137	3228696117376486889	1	2
101	2000001138	-4996633728173344823	1	0
101	2000001139	6676658122434186560	1	17
101	2000001140	971174339332451099	1	9
101	2000001141	-7177800226208002436	1	0
101	2000001142	307236824694356492	1	0
101	2000001143	2550114325481514518	1	0
101	2000001144	4637543035199056940	1	18
101	2000001145	-6230743295456017513	1	11
101	2000001146	7192667341691898816	1	10
101	2000001147	5205638624216220967	1	4
101	2000001148	-6565075019170510868	1	2
101	2000001149	3782058628936311715	1	1
101	2000001150	-3195672186131227094	1	56
101	2000001151	-5334509675145412683	1	1
101	2000001152	7016315370034764348	1	2
101	2000001153	6476892859771650470	1	1
101	2000001154	-3465740592744057548	1	17
101	2000001155	-3037517196981363286	1	8
101	2000001156	1114369319716674198	1	2
101	2000001157	5751062995806576286	1	8
101	2000001158	236766866536675906	1	5
101	2000001159	-1944632089074046664	1	11
101	2000001160	-7752258273955670937	1	3
101	2000001161	-2928071068988228475	1	3
101	2000001162	4994927038042679865	1	3
101	2000001163	8281097770071769022	1	2
101	2000001164	-2960412080906415157	1	10
101	2000001165	-8762168969870978127	1	2
101	2000001166	2243133368614220796	1	5
101	2000001167	1715240283461579933	1	2
101	2000001168	9219794399687963470	1	2
101	2000001169	-804832200965223130	1	1
101	2000001170	-6364009762563688071	1	6
101	2000001171	1816866295334011362	1	4
101	2000001172	3503386237523232980	1	5
101	2000001173	-2297595275933006673	1	3
101	2000001174	-4392497747760791413	1	2
101	2000001175	-1780397812148861499	1	2
101	2000001176	-3264790370237890194	1	2
101	2000001177	-4578522483913369008	1	0
101	2000001178	-8685499480915168784	1	0
101	2000001179	8991301413776457540	1	0
101	2000001180	7145918418903829447	1	1
101	2000001181	-662693263978555887	1	6
101	2000001182	5179246638415980096	1	5
101	2000001183	364245053286702444	1	1851
101	2000001184	-6089330072891960682	1	2
101	2000001185	4974840309390420475	1	0
101	2000001186	-6891234349198819717	1	1
101	2000001187	-1081932165472392682	1	0
101	2000001188	-862757893900596786	1	0
101	2000001189	4761756117214222146	1	0
101	2000001190	-7575432177193452195	1	0
101	2000001191	-4702397548553202652	1	6
101	2000001192	45264677980430591	1	0
101	2000001193	3878448300163266785	1	3
101	2000001194	-9023608126608041850	1	0
101	2000001195	-4416009015091390470	1	1
101	2000001196	7866436034146456517	1	0
101	2000001197	-249521819635086411	1	0
101	2000001198	4501916349915969181	1	0
101	2000001199	2065468963683054448	1	0
101	2000001200	2495145267416411563	1	0
101	2000001201	2015705189481042425	1	0
101	2000001202	-8971579562146055686	1	0
101	2000001203	3703406014009492720	1	0
101	2000001204	7019895460183837475	1	2
101	2000001205	-2813586467317074384	1	0
101	2000001206	-2428216348344006560	1	0
101	2000001207	6553941132847579211	1	0
101	2000001208	-1557936242763417275	1	0
101	2000001209	7643520708485295897	1	0
101	2000001210	7056256915170179218	1	0
101	2000001211	906041612081580848	1	0
101	2000001212	2255057613336132639	1	0
101	2000001213	7239633883044694886	1	0
101	2000001214	3749794823157993345	1	0
101	2000001215	5190530051342802745	1	0
101	2000001216	4822238645918970741	1	0
101	2000001217	4695969812868151224	1	0
101	2000001218	-522817394540157663	1	0
101	2000001219	-2078955599824486635	1	0
101	2000001220	4171638246585978850	1	0
101	2000001221	1813219048352356292	1	0
101	2000001222	4554315221965242666	1	0
101	2000001223	-6833410158936175156	1	0
101	2000001224	-5296972718564795889	1	0
101	2000001225	-1269317059936514769	1	0
101	2000001226	-8468074403158276113	1	0
101	2000001227	7447131582808425736	1	0
101	2000001228	-1332485022344352573	1	0
101	2000001229	5082681135190863564	1	2
101	2000001230	3437836429829108529	1	1
101	2000001231	238092025138455498	1	189
101	2000001232	2849429986525720309	1	1
101	2000001233	1351459559490765744	1	1
101	2000001234	4483650246607868747	1	184
101	2000001235	-2589841752501854624	1	9
101	2000001236	2170570042553337662	1	82
101	2000001237	606864617872302992	1	234
101	2000001238	-2624186850303116539	1	217
101	2000001239	-6857719881763129813	1	1
101	2000001240	-561277564324188454	1	0
101	2000001241	691319980552543269	1	2
101	2000001242	1275445145094723242	1	1
101	2000001243	-3295972363196954376	1	4
101	2000001244	-91721315894732797	1	2
101	2000001245	3092181141534013781	1	0
101	2000001246	3693379232998328949	1	0
101	2000001247	-5922794219681273649	1	0
101	2000001248	1300457086185047770	1	0
101	2000001249	-361911890771219027	1	0
101	2000001250	1925544013814049554	1	0
101	2000001251	2634373055963632102	1	1
101	2000001252	-665776924063090015	1	183
101	2000001253	-5136452061916394765	1	859
101	2000001254	8897796258654925583	1	2657
101	2000001255	-7384987120366478110	1	841
101	2000001256	-5937758128597759100	1	92
101	2000001257	-1416300145659030343	1	2002
101	2000001258	4258774145876756265	1	607
101	2000001259	4223747915072276681	1	605
101	2000001260	-8824255370276270199	1	0
101	2000001261	3177323779712560580	1	0
101	2000001262	3983000563565984390	1	0
101	2000001263	-7000959205335803701	1	0
101	2000001264	-5005933384339086461	1	0
101	2000001265	2698705444242427190	1	182
101	2000001266	687857937432763853	1	0
101	2000001267	-5597116519052833346	1	138
101	2000001268	-2591488856581830582	1	64
101	2000001269	-6544790822615003457	1	469
101	2000001270	-2825558108184742708	1	463
101	2000001271	1168994223344323518	1	24
101	2000001272	-2032319083738454373	1	1
101	2000001273	1796867783899292493	1	0
101	2000001274	-3686252796771842845	1	1
101	2000001275	-2270158545202692171	1	0
101	2000001276	8730914225656170048	1	6
101	2000001277	-8215178805094666448	1	4
101	2000001278	-5291778478237688275	1	0
101	2000001279	1525258728227518028	1	0
101	2000001280	5453959019869486766	1	0
101	2000001281	-7847209498562172930	1	0
101	2000001282	823917457326975110	1	0
101	2000001283	-1036781908657052566	1	0
101	2000001284	7512737960426415814	1	0
101	2000001285	-2550841874831921724	1	0
101	2000001286	-6338598668737218660	1	0
101	2000001287	-327137777713093119	1	0
101	2000001288	9150871031119415570	1	0
101	2000001289	-684804499359966456	1	1
101	2000001290	-6157280172931092825	1	23
101	2000001291	-2737519622774481858	1	7
101	2000001292	1179132302154905766	1	4
101	2000001293	-3143703892714601300	1	7
101	2000001294	-6785070894776717730	1	3
101	2000001295	7748983039484611270	1	3
101	2000001296	4952726493009253463	1	3
101	2000001297	-1852319116449686914	1	3
101	2000001298	-8978907799821498318	1	3
101	2000001299	7959537034108505793	1	3
101	2000001300	-141851571656334866	1	2
101	2000001301	7626764079958247388	1	3
101	2000001302	7836664940262010163	1	3
101	2000001303	4776222105303431085	1	3
101	2000001304	2104993403377692519	1	3
101	2000001305	8524398317037426129	1	3
101	2000001306	-3020259999343376675	1	4
101	2000001307	-6410872747388853356	1	7
101	2000001308	-2734078166033749347	1	7
101	2000001309	696890939607808317	1	4
101	2000001310	-5248779109464195655	1	3
101	2000001311	3635981740501520847	1	3
101	2000001312	7868981472740619149	1	3
101	2000001313	-5382392792071956320	1	3
101	2000001314	4722228480134289451	1	3
101	2000001315	2258045652370874348	1	3
101	2000001316	7458238141391050739	1	3
101	2000001317	5324069311841853375	1	3
101	2000001318	7682350723153265293	1	3
101	2000001319	-255479049949798456	1	10
101	2000001320	8244428772910906594	1	1
101	2000001321	7802840229146774501	1	0
101	2000001322	2672924392723299654	1	0
101	2000001323	3420410409558794600	1	0
101	2000001324	-1879805963535627571	1	0
101	2000001325	7902977540104998009	1	1
101	2000001326	987872124572520775	1	0
101	2000001327	2486188709265545329	1	1
101	2000001328	-7223695806028118107	1	0
101	2000001329	-952303128732388034	1	8
101	2000001330	6220656556765335703	1	6
101	2000001331	-7168421973051444652	1	6
101	2000001332	-7929991792431856222	1	5
101	2000001333	-3009228473796984114	1	7
101	2000001334	7716124063300007096	1	6
101	2000001335	5536513000467519387	1	6
101	2000001336	2628421056183551099	1	5
101	2000001337	-92720985729499244	1	6
101	2000001338	-8226364803093571800	1	6
101	2000001339	-7741358946032307142	1	5
101	2000001340	6920604210740085850	1	5
101	2000001341	-1483961530525751665	1	3
101	2000001342	6213761762685107727	1	1
101	2000001343	-7601845516294979005	1	3
101	2000001344	-8937175801598297559	1	0
101	2000001345	2640566765996170294	1	0
101	2000001346	8144422378637737611	1	0
101	2000001347	8023933319764954538	1	0
101	2000001348	-8310467304190011765	1	0
101	2000001349	5122595439880943005	1	0
101	2000001350	-7367917403558354336	1	2
101	2000001351	8409376315647180451	1	8
101	2000001352	-9085754636720657457	1	1
101	2000001353	-614415171202939901	1	0
101	2000001354	-2457224510879870735	1	0
101	2000001355	-5248650230956437074	1	0
101	2000001356	3638941178979011375	1	0
101	2000001357	9116659678081060652	1	0
101	2000001358	7943931625662181984	1	16
101	2000001359	7765117299931483029	1	195
101	2000001360	-761294447628058446	1	45
101	2000001361	6057698298866825827	1	62
101	2000001362	5222284899394960018	1	45
101	2000001363	5405163203147116641	1	5557
101	2000001364	8702385181061615546	1	9205
101	2000001365	8845825461403835908	1	2255
101	2000001366	3592681968420981920	1	538
101	2000001367	3740543862357774599	1	523
101	2000001368	6795260181976179461	1	765
102	2000000000	-5168067613992917628	1	56
102	2000000001	1658048510990355987	1	177
102	2000000002	395155716920624105	1	17
102	2000000003	2105868547303546195	1	1
102	2000000004	4168344068604504176	1	23
102	2000000005	-599379416200460755	1	5
102	2000000006	5865893109958142336	1	16
102	2000000007	-1014448920728114714	1	9
102	2000000008	-5914716010989158113	1	2
102	2000000009	4226214848212862725	1	2
102	2000000010	4142698781433296355	1	4
102	2000000011	891115598539416633	1	0
102	2000000012	-8247447631318334064	1	2
102	2000000013	5039604955451317509	1	1
102	2000000014	320849263338300981	1	0
102	2000000015	-5488696493139541078	1	0
102	2000000016	1097231189897963555	1	0
102	2000000017	-7740681692428656038	1	1
102	2000000018	6553368331669324775	1	0
102	2000000019	-8986029802410366237	1	0
102	2000000020	1605842430492936233	1	0
102	2000000021	104097758823551538	1	2
102	2000000022	5914280589009734834	1	2
102	2000000023	4213147678250946958	1	2
102	2000000024	6183954501267872216	1	0
102	2000000025	-5205656477440234433	1	1
102	2000000026	-7493652208583830495	1	2
102	2000000027	2908739203109759186	1	0
102	2000000028	5130518625500238005	1	0
102	2000000029	7882878838845453136	1	0
102	2000000030	6892766825010809023	1	0
102	2000000031	-5663271173113375380	1	0
102	2000000032	9154034748061152465	1	0
102	2000000033	-1357977716921926776	1	1
102	2000000034	-6629809875920628607	1	0
102	2000000035	-5312011533091652985	1	2
102	2000000036	718348107055543106	1	9
102	2000000037	-1157373951113888645	1	1
102	2000000038	3837923844096457385	1	4
102	2000000039	-7864049997018424615	1	1
102	2000000040	-2628665152080434732	1	0
102	2000000041	7784828775858321386	1	0
102	2000000042	8687322539772689954	1	0
102	2000000043	-8965823899831416089	1	0
102	2000000044	-5014417892241219914	1	0
102	2000000045	5317262931568349904	1	9
102	2000000046	982545823736371990	1	11
102	2000000047	-6375807517106078134	1	1
102	2000000048	3884006348627733517	1	2
102	2000000049	3773868157680640943	1	0
102	2000000050	4133740822589071545	1	4
102	2000000051	-631540076145704700	1	3
102	2000000052	471873135801994195	1	175
102	2000000053	-8211194052941157511	1	0
102	2000000054	4869087816672658090	1	7
102	2000000055	7307247278838092431	1	6
102	2000000056	-6754945327394567629	1	1
102	2000000057	-7530077675173574284	1	3
102	2000000058	-6813819657005622371	1	0
102	2000000059	5013891203746348224	1	0
102	2000000060	8139150691954259558	1	0
102	2000000061	-6863013941068350251	1	0
102	2000000062	8099482474164728756	1	2
102	2000000063	3502479247353265537	1	6
102	2000000064	4421890388133832713	1	1
102	2000000065	-1225547086238709623	1	1
102	2000000066	6004521876572272844	1	0
102	2000000067	-8579785066513323788	1	2
102	2000000068	5084515830392146249	1	5
102	2000000069	-7631616222065932609	1	26
102	2000000070	4054285518414936991	1	0
102	2000000071	855622501930378268	1	4
102	2000000072	4822464996449682968	1	1
102	2000000073	4804759450788955081	1	0
102	2000000074	2615522436833709908	1	0
102	2000000075	-5893936263433988855	1	0
102	2000000076	-2806122811888598155	1	0
102	2000000077	-323908055081131550	1	0
102	2000000078	-4228791055132281238	1	0
102	2000000079	4585975189901988855	1	0
102	2000000080	-1562883777249695314	1	0
102	2000000081	-2261324999257812422	1	0
102	2000000082	385756262405517557	1	0
102	2000000083	-1976503402141874080	1	0
102	2000000084	4525651972397617056	1	0
102	2000000085	828119402709425352	1	0
102	2000000086	5598680601370150278	1	0
102	2000000087	-3290616611505052031	1	18
102	2000000088	-3075720102990604265	1	0
102	2000000089	-4205356829618576577	1	0
102	2000000090	1541999056923217352	1	0
102	2000000091	-6729386337864249509	1	0
102	2000000092	-1636241169476163454	1	1
102	2000000093	-842028361149352176	1	0
102	2000000094	-5001296628537403263	1	0
102	2000000095	-4895682748520660873	1	90
102	2000000096	9207039181192489174	1	5
102	2000000097	265299714377165253	1	1
102	2000000098	1850931895478759887	1	35
102	2000000099	-3858185599271528368	1	13
102	2000000100	5369156447176937088	1	4
102	2000000101	2682672937639856679	1	3
102	2000000102	7644799529120265731	1	9
102	2000000103	546148726491252312	1	3
102	2000000104	-6925810533630939515	1	2
102	2000000105	2801260725862711702	1	2
102	2000000106	3985171750648239103	1	2
102	2000000107	-2094885574236634978	1	1
102	2000000108	-7293008259388994363	1	2
102	2000000109	8289558281556610212	1	19
102	2000000110	-8359869987379225846	1	7
102	2000000111	4643082334228461166	1	5
102	2000000112	-7182649009264205024	1	4
102	2000000113	-262372815853356321	1	5
102	2000000114	2853305180478063174	1	5
102	2000000115	273740604063040256	1	5
102	2000000116	5910254892637388891	1	53
102	2000000117	7296555989703615511	1	6
102	2000000118	-1647803935715495851	1	13
102	2000000119	-7216378367928879894	1	5
102	2000000120	2642972094844077548	1	5
102	2000000121	6061745905256248282	1	5
102	2000000122	4178047494179091499	1	13
102	2000000123	7899792671160188869	1	11
102	2000000124	-7946802209278770662	1	7
102	2000000125	-2095179500710035246	1	7
102	2000000126	-5370593418204654093	1	5
102	2000000127	5970637498973245149	1	6
102	2000000128	-5696787878964117539	1	5
102	2000000129	3886766662433091017	1	10
102	2000000130	8292494691086061859	1	8
102	2000000131	4985031533047976606	1	7
102	2000000132	8549850915877672668	1	9
102	2000000133	8229997000000531653	1	7
102	2000000134	-9128183645133246639	1	7
102	2000000135	7218161957043746000	1	7
102	2000000136	-7197118442584039090	1	13
102	2000000137	2498548561942185946	1	6
102	2000000138	8286629246610762820	1	9
102	2000000139	-1174808756205235933	1	7
102	2000000140	-8304354904086372845	1	6
102	2000000141	-901036977350955796	1	13
102	2000000142	6165186261284916642	1	41
102	2000000143	5961349759012781409	1	70
102	2000000144	3358260765246435230	1	6
102	2000000145	3541929903117895034	1	5
102	2000000146	-1084510063449187158	1	9
102	2000000147	3457924317783329288	1	9
102	2000000148	8805228457669272158	1	2
102	2000000149	5062942869313641847	1	4
102	2000000150	8881830009048021057	1	11
102	2000000151	6762983883462077005	1	3
102	2000000152	-5747231119492549359	1	2
102	2000000153	3222081366833223268	1	8
102	2000000154	2466633683436661857	1	4
102	2000000155	-1575323095389685462	1	4
102	2000000156	-802959555982311160	1	1
102	2000000157	-2304382874982447364	1	16
102	2000000158	-7215277425977975246	1	1
102	2000000159	-1094041455508475148	1	1
102	2000000160	3421573324132374923	1	1
102	2000000161	-9207514556493929158	1	0
102	2000000162	4504696974646285517	1	0
102	2000000163	5780058753660829978	1	0
102	2000000164	5788044238413357566	1	6
102	2000000165	45366133087923521	1	3
102	2000000166	-3107720997913478708	1	0
102	2000000167	-7414640722507565204	1	3
102	2000000168	2728663142375709036	1	2
102	2000000169	5532191644254049702	1	0
102	2000000170	-2575506855031989672	1	0
102	2000000171	3913174321725019164	1	0
102	2000000172	-6325375061704290982	1	3
102	2000000173	1952829678854231830	1	0
102	2000000174	-7714358181202295009	1	0
102	2000000175	167381447430374963	1	89
102	2000000176	-7156862266043976418	1	26
102	2000000177	7453602060532121344	1	55
102	2000000178	2048672468661446044	1	0
102	2000000179	-5313316152668899945	1	0
102	2000000180	6546417840909422353	1	0
102	2000000181	7998291862378205807	1	0
102	2000000182	1583629675819367816	1	18
102	2000000183	6015330456467304167	1	1
102	2000000184	-3922533019978291516	1	1
102	2000000185	6469926083620759826	1	1
102	2000000186	-6197704211498601688	1	24
102	2000000187	7506018132291755046	1	10
102	2000000188	-2854098181133654390	1	1
102	2000000189	5580666055292867697	1	1
102	2000000190	-5586930227402413306	1	1
102	2000000191	-5863135281082852428	1	2
102	2000000192	6876094986674014987	1	11
102	2000000193	-4387068365724119821	1	0
102	2000000194	7685242590091901238	1	5
102	2000000195	-2846826406673728752	1	42
102	2000000196	9174150359974885957	1	26
102	2000000197	-2872144303876561253	1	19
102	2000000198	-3668471866226188888	1	42
102	2000000199	2286211212203948379	1	35
102	2000000200	3876868695864254709	1	25
102	2000000201	-8982279052801127019	1	33
102	2000000202	-1191290804749559924	1	37
102	2000000203	3188356211687288195	1	21
102	2000000204	8158340901452721225	1	131
102	2000000205	6159620885790173160	1	146
102	2000000206	3132744306913834556	1	14
102	2000000207	-3207103653603210855	1	10
102	2000000208	-8222704019872743726	1	6
102	2000000209	-8768095674480220028	1	0
102	2000000210	4768003828651437716	1	0
102	2000000211	8906314032266130370	1	2
102	2000000212	-7100702980283025836	1	1
102	2000000213	1344233066732476800	1	1
102	2000000214	7863800826459049259	1	1
102	2000000215	-5635970143258691064	1	0
102	2000000216	399603175506464863	1	1
102	2000000217	-4159700018826741568	1	0
102	2000000218	-1217956621720234165	1	0
102	2000000219	-4379048972556788231	1	3
102	2000000220	6910590723172147604	1	1
102	2000000221	-5436223351934266950	1	0
102	2000000222	3385906740431711903	1	0
102	2000000223	1015688014506287140	1	0
102	2000000224	-4545065987677879872	1	9
102	2000000225	-1690653735689708521	1	1
102	2000000226	9152719559837512697	1	1
102	2000000227	2112934594205575010	1	0
102	2000000228	-6921788027242763511	1	1
102	2000000229	9026668089164890926	1	0
102	2000000230	7835985019669643007	1	0
102	2000000231	1748930001085728374	1	0
102	2000000232	-4870757489143658629	1	0
102	2000000233	-8946682362426085462	1	0
102	2000000234	6482940291938471003	1	1
102	2000000235	-5100029622945035865	1	1
102	2000000236	5861494255440031174	1	0
102	2000000237	-3704034302743523421	1	0
102	2000000238	-3910353358171428391	1	0
102	2000000239	7178471542052155939	1	0
102	2000000240	-4846440185093465276	1	0
102	2000000241	-5098932985785461169	1	0
102	2000000242	3253163512775453786	1	0
102	2000000243	-2790709064176103988	1	1
102	2000000244	871906643331371361	1	0
102	2000000245	-4909190956136147169	1	0
102	2000000246	-811238283709010921	1	0
102	2000000247	-3044470657175993418	1	4
102	2000000248	7882824793751932003	1	0
102	2000000249	7742639209789501280	1	0
102	2000000250	-2046409902734848786	1	0
102	2000000251	-5682229402934137803	1	0
102	2000000252	-6243293462249496788	1	1
102	2000000253	8170965163082980576	1	0
102	2000000254	-1626235145649623620	1	0
102	2000000255	854370132355022024	1	0
102	2000000256	3273353310598347926	1	0
102	2000000257	8901378153430543944	1	1
102	2000000258	-6177029893441046382	1	0
102	2000000259	-7480954548352835387	1	0
102	2000000260	-3716360097816402166	1	0
102	2000000261	-1514631603694905049	1	0
102	2000000262	5140311844596731374	1	0
102	2000000263	671267312631720891	1	0
102	2000000264	-8631849075709006578	1	2
102	2000000265	3441965872625483974	1	0
102	2000000266	-5685808563646212420	1	0
102	2000000267	-5859128146916308836	1	0
102	2000000268	42973866600002177	1	1
102	2000000269	3442892229187295721	1	0
102	2000000270	-8635253897124206828	1	0
102	2000000271	5460041067622205790	1	0
102	2000000272	8068526363439912750	1	0
102	2000000273	-3276954727298532722	1	0
102	2000000274	-3844892461784491471	1	0
102	2000000275	8984070560824590680	1	0
102	2000000276	-6791269373554175731	1	1
102	2000000277	5413119706348108665	1	0
102	2000000278	-5649409396766215112	1	0
102	2000000279	-2956507839275432988	1	0
102	2000000280	6304524793301695953	1	0
102	2000000281	-2531294556859143024	1	0
102	2000000282	-3591361680031440861	1	1
102	2000000283	-4726818166408084722	1	0
102	2000000284	9160233208864691179	1	1
102	2000000285	8439687064434765458	1	0
102	2000000286	333483086096992788	1	0
102	2000000287	-2294112922519809294	1	1
102	2000000288	-1782386555252545714	1	0
102	2000000289	-2319932335302408233	1	1
102	2000000290	-6941344528592049593	1	0
102	2000000291	-8387929219434347608	1	0
102	2000000292	5534209550501235227	1	0
102	2000000293	-7842144647537605462	1	0
102	2000000294	3529563026656807306	1	0
102	2000000295	3292015599141719441	1	0
102	2000000296	-1956624619424199379	1	0
102	2000000297	2216113875507654384	1	0
102	2000000298	-6962178826515084706	1	2
102	2000000299	8840354425581028533	1	0
102	2000000300	-4659232289303006199	1	0
102	2000000301	2893223015212878224	1	11
102	2000000302	500549125022718037	1	9
102	2000000303	1596753244653843244	1	7
102	2000000304	2317779630936083971	1	6
102	2000000305	-4790867754608461845	1	5
102	2000000306	8355975946991213234	1	4
102	2000000307	4525469924466266750	1	67
102	2000000308	8339961779989768797	1	449
102	2000000309	6768425885042804800	1	394
102	2000000310	-3702124768431439213	1	2
102	2000000311	-2151332099485546499	1	0
102	2000000312	6805988862252946054	1	1
102	2000000313	4186957571204407323	1	0
102	2000000314	-1137552624966346092	1	0
102	2000000315	-6843401644120111781	1	0
102	2000000316	-4775450474354578645	1	0
102	2000000317	673073708176228709	1	1
102	2000000318	-6042258967121502890	1	1
102	2000000319	5454844165586025251	1	1
102	2000000320	4781022324913180018	1	163
102	2000000321	7198195242312020334	1	1
102	2000000322	8061551208747540058	1	0
102	2000000323	975291638248038318	1	1
102	2000000324	5296186274952516017	1	1
102	2000000325	-7220900720455481494	1	0
102	2000000326	4598769653297822368	1	0
102	2000000327	5557147014809399260	1	1
102	2000000328	-6300936569192378386	1	1
102	2000000329	6583713035894075762	1	1
102	2000000330	-7148742707063303148	1	6
102	2000000331	6891133056092466157	1	5
102	2000000332	8847504368228341853	1	3
102	2000000333	-769850871404066714	1	5
102	2000000334	8010487579726324572	1	4
102	2000000335	-1281815806852441397	1	3
102	2000000336	1938521020609095059	1	1
102	2000000337	3523884679671725957	1	5
102	2000000338	810398108306760348	1	11
102	2000000339	-3753456124963804734	1	10
102	2000000340	9055249480909265145	1	1
102	2000000341	1376848864724890411	1	2
102	2000000342	-7249976424530137073	1	3
102	2000000343	4964813406853494573	1	1
102	2000000344	-4591010665574936763	1	2
102	2000000345	287894396417678741	1	3
102	2000000346	-3260839014246629641	1	157
102	2000000347	4089557322633378077	1	4
102	2000000348	-37434540835908810	1	0
102	2000000349	-6012374541873459228	1	1
102	2000000350	-8900759627960524397	1	0
102	2000000351	-6743800908559635352	1	0
102	2000000352	8199428507212141948	1	0
102	2000000353	-7201485080435899140	1	0
102	2000000354	1921674646476416089	1	0
102	2000000355	-1963231258293348397	1	0
102	2000000356	-8218325220138309565	1	0
102	2000000357	-5848613183465042385	1	0
102	2000000358	7878477018103303313	1	0
102	2000000359	-652171486573031821	1	0
102	2000000360	-6985289698676835606	1	0
102	2000000361	-5041753597538503662	1	0
102	2000000362	3530066322638971749	1	0
102	2000000363	-2370561505487284775	1	0
102	2000000364	1672842826120172223	1	0
102	2000000365	-2389231846021142751	1	0
102	2000000366	-5707071873651388052	1	0
102	2000000367	5504638158992425192	1	0
102	2000000368	-3103166860339059077	1	0
102	2000000369	-8762091848778466957	1	0
102	2000000370	-2138427480696216105	1	0
102	2000000371	-8727409099088109660	1	1
102	2000000372	-5455537310022155887	1	2
102	2000000373	-167802844271584258	1	0
102	2000000374	2198556615269313949	1	0
102	2000000375	7040673462681810840	1	0
102	2000000376	123339232929051273	1	0
102	2000000377	-3971936715945561297	1	0
102	2000000378	-5889541757896086778	1	0
102	2000000379	7380966007935825479	1	0
102	2000000380	5345430275619616017	1	0
102	2000000381	-3268371542770224896	1	0
102	2000000382	-9150411862071801368	1	0
102	2000000383	-8627233018830102918	1	0
102	2000000384	2383175652925770064	1	0
102	2000000385	-3529455259661086276	1	0
102	2000000386	8112924697463551049	1	0
102	2000000387	-3889480022973573131	1	12
102	2000000388	-554800153383183542	1	37
102	2000000389	3174605870365863445	1	7
102	2000000390	-6010461173274098506	1	4
102	2000000391	3972174041477632771	1	9
102	2000000392	-599618953858237035	1	1
102	2000000393	-3332655978695007023	1	4
102	2000000394	3334900419860551377	1	1
102	2000000395	-8391786289264016656	1	1
102	2000000396	2324740851889752724	1	14
102	2000000397	-1306535233403231905	1	7
102	2000000398	3691910877796480479	1	3
102	2000000399	4101574638442679856	1	4
102	2000000400	2528460505267493704	1	1
102	2000000401	3868823698102365907	1	26
102	2000000402	1210847126746082376	1	2
102	2000000403	6274690045188883515	1	2
102	2000000404	2448275589862489061	1	2
102	2000000405	1019708284290908756	1	1
102	2000000406	5731224940909260024	1	1
102	2000000407	-4461578282072854891	1	1
102	2000000408	4422570313003487721	1	1
102	2000000409	-6975057738140130921	1	0
102	2000000410	2583041290795623063	1	1
102	2000000411	4554372258448489687	1	0
102	2000000412	4676693325818911216	1	0
102	2000000413	-6899431719471549278	1	4
102	2000000414	3509701785538232647	1	1
102	2000000415	-9072473139632003956	1	4
102	2000000416	-8218772127320035485	1	0
102	2000000417	-1340594121368301723	1	3
102	2000000418	-6617398307302016737	1	2
102	2000000419	7377611215149686245	1	3
102	2000000420	-8717890372998608465	1	1
102	2000000421	-3768139355512319682	1	1
102	2000000422	186989653214630812	1	1
102	2000000423	-8575991715294204431	1	1
102	2000000424	-2135398279124016704	1	1
102	2000000425	3745991302157209440	1	0
102	2000000426	7200778840955335664	1	0
102	2000000427	5403114043803834687	1	1
102	2000000428	7471078443531567468	1	0
102	2000000429	-6516337435024933937	1	0
102	2000000430	-8157418118392356068	1	0
102	2000000431	-8941669507633053478	1	1
102	2000000432	1750220706064829111	1	0
102	2000000433	-323380473992954954	1	0
102	2000000434	-4750834435677326447	1	0
102	2000000435	6395554917994421796	1	2
102	2000000436	-2393752537121269767	1	0
102	2000000437	-6232339777137704434	1	0
102	2000000438	-520295747944787748	1	1
102	2000000439	8935027621324685126	1	1
102	2000000440	1819288246643318899	1	1
102	2000000441	9082917390085156165	1	0
102	2000000442	2677094157800687234	1	0
102	2000000443	-5118235684681422184	1	0
102	2000000444	-7672359881715126002	1	0
102	2000000445	-3274095225854377694	1	3
102	2000000446	4773013725081228387	1	0
102	2000000447	8295892158328255943	1	0
102	2000000448	5946293114760361894	1	2
102	2000000449	-2857515491886374742	1	7
102	2000000450	-9016000243843539149	1	0
102	2000000451	7991943861722550330	1	2
102	2000000452	-5032889823836522363	1	2
102	2000000453	-1069215540778290849	1	0
102	2000000454	7933044701474115079	1	0
102	2000000455	3383483138513852096	1	0
102	2000000456	-5021294807930804702	1	1
102	2000000457	3232398040969668304	1	1
102	2000000458	5701699701766514880	1	3
102	2000000459	1079777846125395147	1	2
102	2000000460	-4757185636571062174	1	1
102	2000000461	631308306954226985	1	185
102	2000000462	6592849061380933631	1	1
102	2000000463	100475979119385272	1	0
102	2000000464	4488047058959874712	1	2
102	2000000465	6235711471212629325	1	0
102	2000000466	3450623954380032235	1	0
102	2000000467	-7805929135540215937	1	0
102	2000000468	-2912815792102701509	1	0
102	2000000469	-3149577814460868489	1	0
102	2000000470	2419264815177948334	1	0
102	2000000471	2892885172302753480	1	0
102	2000000472	2609711005454547867	1	0
102	2000000473	-517343203919092306	1	0
102	2000000474	-9124560809556394670	1	0
102	2000000475	-8330373889145914487	1	0
102	2000000476	8747320628437539152	1	0
102	2000000477	-6945858700441151447	1	0
102	2000000478	-6081354969982681999	1	0
102	2000000479	4035584893480932499	1	0
102	2000000480	-7056374640302649604	1	0
102	2000000481	-4791221260792473098	1	0
102	2000000482	-5333581857906755938	1	0
102	2000000483	657025011518556863	1	0
102	2000000484	-8970379998755300367	1	0
102	2000000485	-7040402825184554156	1	0
102	2000000486	-1460346436665267622	1	1
102	2000000487	-9022769976017791560	1	1
102	2000000488	8308133133012897805	1	0
102	2000000489	-5498163394167153218	1	0
102	2000000490	5204670132024648480	1	1
102	2000000491	-5006680664044913737	1	0
102	2000000492	8218793679501148847	1	0
102	2000000493	-3184534676668196556	1	0
102	2000000494	-1507875266406929359	1	0
102	2000000495	6412966820676540751	1	0
102	2000000496	5746705507760678982	1	0
102	2000000497	-1400974169623107081	1	0
102	2000000498	6386329771030373937	1	0
102	2000000499	1929673210871152925	1	0
102	2000000500	302047598451694000	1	16
102	2000000501	2062383995271269786	1	0
102	2000000502	-3325137484275582249	1	0
102	2000000503	3720236562660988131	1	0
102	2000000504	5455646959622234629	1	0
102	2000000505	-8946073386512960103	1	0
102	2000000506	-6933499149927628326	1	0
102	2000000507	3619311731473916220	1	0
102	2000000508	-3540429236196209402	1	0
102	2000000509	1680103686424152264	1	1
102	2000000510	5550703416682629513	1	0
102	2000000511	-1261190560869627214	1	0
102	2000000512	-7382620743575113589	1	0
102	2000000513	-6481782723578544090	1	0
102	2000000514	-6271626231804380759	1	0
102	2000000515	-2362168989856320897	1	0
102	2000000516	-3747520104157667041	1	0
102	2000000517	-6274934036837929527	1	0
102	2000000518	9063175315430654317	1	0
102	2000000519	3600368723188857852	1	0
102	2000001365	8845825461403835908	1	2244
102	2000001366	3592681968420981920	1	529
102	2000001367	3740543862357774599	1	511
102	2000001368	6795260181976179461	1	707
102	2000000520	-3275278053552906791	1	0
102	2000000521	-4089443421389216307	1	0
102	2000000522	2209504073079976925	1	0
102	2000000523	6742207834329338799	1	0
102	2000000524	-1931136746910890635	1	0
102	2000000525	-8923877033546041150	1	2
102	2000000526	4361595946815463385	1	1
102	2000000527	-7126657288987151105	1	3
102	2000000528	-8474275296072444440	1	0
102	2000000529	-829046258088547010	1	0
102	2000000530	-3429772387199373398	1	0
102	2000000531	4287622489691514106	1	0
102	2000000532	4305927349006483485	1	1
102	2000000533	-5891975601941594263	1	0
102	2000000534	3552664310779555538	1	1
102	2000000535	77098920388313739	1	0
102	2000000536	1008163528827344863	1	0
102	2000000537	2378735368864199773	1	0
102	2000000538	-411873028061658547	1	0
102	2000000539	-4181691547645504898	1	0
102	2000000540	375420428497839569	1	0
102	2000000541	-4814854482884217950	1	0
102	2000000542	-5052095623130965223	1	0
102	2000000543	8665316472139466444	1	0
102	2000000544	-2352759059289503719	1	0
102	2000000545	-9185417756159682726	1	0
102	2000000546	-2537876146835428001	1	0
102	2000000547	1988307758015769345	1	0
102	2000000548	7375152660932761054	1	0
102	2000000549	-2204572513767827810	1	1
102	2000000550	-4083001903083056848	1	1
102	2000000551	-6417616407480563538	1	0
102	2000000552	-1090872573128234889	1	2
102	2000000553	-1870455361179779121	1	0
102	2000000554	-3911209253402778809	1	0
102	2000000555	1819032009935960307	1	0
102	2000000556	-7772786161986220840	1	0
102	2000000557	2887177096307434411	1	0
102	2000000558	-8330862128140684673	1	0
102	2000000559	-3752822565344380246	1	0
102	2000000560	-2642196237391216400	1	0
102	2000000561	-7514727290563466446	1	0
102	2000000562	-8803297981895917015	1	0
102	2000000563	1377339183922792491	1	0
102	2000000564	4139768908245332268	1	0
102	2000000565	-7258685998976334159	1	0
102	2000000566	8476909923968604590	1	0
102	2000000567	-6157168172706026864	1	0
102	2000000568	-3602472103318952577	1	0
102	2000000569	-6377809419111902802	1	0
102	2000000570	5629305678908643644	1	1
102	2000000571	1466672490978449442	1	0
102	2000000572	3936942997757393197	1	0
102	2000000573	-4021020179368974406	1	1
102	2000000574	-6841055803091445449	1	0
102	2000000575	3254722748319759749	1	0
102	2000000576	8770879149278552638	1	0
102	2000000577	-547175503758867039	1	18
102	2000000578	8267360228842387378	1	17
102	2000000579	-8840173634241471262	1	1
102	2000000580	-5353300839657544953	1	0
102	2000000581	-674673763740317037	1	0
102	2000000582	-316447274411310824	1	0
102	2000000583	-1591481693817104529	1	0
102	2000000584	2223566409522421057	1	0
102	2000000585	-1899365803510401341	1	0
102	2000000586	-1203559981284246877	1	0
102	2000000587	5783387881951179883	1	0
102	2000000588	8035979234755379503	1	0
102	2000000589	158014887412681138	1	0
102	2000000590	108268604432865115	1	0
102	2000000591	4027233132685793832	1	0
102	2000000592	-8836624144465935612	1	0
102	2000000593	224841885053618169	1	0
102	2000000594	-473344733180064987	1	1
102	2000000595	7303097711185187609	1	1
102	2000000596	6410203933308010749	1	0
102	2000000597	7144604583276889671	1	0
102	2000000598	-923832771737137593	1	0
102	2000000599	472257259985170296	1	0
102	2000000600	-7183873401417815368	1	0
102	2000000601	5639561393726250596	1	0
102	2000000602	-748429510391056660	1	0
102	2000000603	-1471821691389305120	1	0
102	2000000604	-1244110861695361205	1	0
102	2000000605	1849356259714524830	1	0
102	2000000606	5372648116563306155	1	1
102	2000000607	3788322937277224048	1	1
102	2000000608	1741724177889364421	1	1
102	2000000609	-1544374647241629962	1	0
102	2000000610	2270205132328204056	1	0
102	2000000611	4570899514858431606	1	0
102	2000000612	-1151592955510978112	1	0
102	2000000613	3641081747835851970	1	0
102	2000000614	3618095136331371696	1	0
102	2000000615	-6147070407927747851	1	0
102	2000000616	-8482464526912559302	1	0
102	2000000617	1454769167281619589	1	0
102	2000000618	-7160657270513693954	1	0
102	2000000619	-5707501221620982254	1	0
102	2000000620	-3393924028984365802	1	0
102	2000000621	218113030405659023	1	0
102	2000000622	-7326307373869845509	1	3
102	2000000623	8994507315322889238	1	1
102	2000000624	7113924106942583828	1	0
102	2000000625	-1178101138277602910	1	0
102	2000000626	6683456173721167212	1	0
102	2000000627	-5187881189794042804	1	0
102	2000000628	-5866543025278487313	1	1
102	2000000629	-1733326771329823463	1	0
102	2000000630	-7883477971508375745	1	0
102	2000000631	7861568514571853282	1	0
102	2000000632	3976811048074606695	1	0
102	2000000633	-7657782823826474449	1	1
102	2000000634	8033395148388329747	1	1
102	2000000635	7521579517602025726	1	0
102	2000000636	5133758865275727318	1	0
102	2000000637	353410713350147686	1	0
102	2000000638	4370706321304864293	1	0
102	2000000639	-604599232154294899	1	0
102	2000000640	4200379726516780142	1	0
102	2000000641	-4856317104683875529	1	0
102	2000000642	2948843064339980442	1	0
102	2000000643	5226501621194592299	1	0
102	2000000644	-3749193557444209145	1	0
102	2000000645	-3068262110343687907	1	0
102	2000000646	-7371290191581905607	1	0
102	2000000647	-1497134584702649418	1	0
102	2000000648	4836779721170942861	1	0
102	2000000649	2330984446180266726	1	1
102	2000000650	-4941497842926012142	1	1
102	2000000651	3850299087064257896	1	1
102	2000000652	1379885231265209318	1	0
102	2000000653	4730900279908313531	1	0
102	2000000654	-6692942897719607452	1	0
102	2000000655	6596385218658280824	1	0
102	2000000656	7742280597781519260	1	0
102	2000000657	-4939379533109856050	1	1
102	2000000658	-559271542322089807	1	0
102	2000000659	-4988170037811856119	1	0
102	2000000660	-1642963215527835214	1	0
102	2000000661	160470546533425842	1	0
102	2000000662	-4597536811667411047	1	0
102	2000000663	-5225835116227179315	1	0
102	2000000664	2012674202134181162	1	0
102	2000000665	9219722906344172547	1	0
102	2000000666	3789344299255261546	1	0
102	2000000667	-1110379169338656866	1	0
102	2000000668	-3259485908059675943	1	0
102	2000000669	7433331124027422644	1	0
102	2000000670	7999208031922245120	1	1
102	2000000671	5805668549733399878	1	1
102	2000000672	3314571711313360320	1	0
102	2000000673	-3548259621211206315	1	0
102	2000000674	4746829911096675271	1	0
102	2000000675	-7623144499908919361	1	0
102	2000000676	-6723657192789197229	1	0
102	2000000677	-6335971593227921026	1	0
102	2000000678	-5775568776361052001	1	0
102	2000000679	-8070008019278497384	1	0
102	2000000680	5679072309791352125	1	0
102	2000000681	8876830551412023741	1	0
102	2000000682	-3460807262607593561	1	0
102	2000000683	3293389237072111462	1	0
102	2000000684	-6436209041997647296	1	0
102	2000000685	6548174634444797682	1	0
102	2000000686	-8522120391317385162	1	0
102	2000000687	777704923058060297	1	0
102	2000000688	-900840466130694104	1	0
102	2000000689	4832504558073963968	1	1
102	2000000690	-8381922247839435305	1	0
102	2000000691	-704062481181072710	1	0
102	2000000692	2020796491414859841	1	0
102	2000000693	-2290156110695935850	1	0
102	2000000694	2302167374604391005	1	0
102	2000000695	-5674698766906137194	1	0
102	2000000696	-9183290367865991264	1	0
102	2000000697	7498069002045135382	1	0
102	2000000698	8894193892046569413	1	0
102	2000000699	-7704236595691425209	1	0
102	2000000700	7579344795684748176	1	0
102	2000000701	6996668969732783144	1	0
102	2000000702	-8009200561564919891	1	0
102	2000000703	5603297846530487325	1	0
102	2000000704	3274810119064705259	1	0
102	2000000705	3146147149573717354	1	2
102	2000000706	-2228400562854313153	1	0
102	2000000707	-8781065821573446522	1	4
102	2000000708	-6650893164806474862	1	0
102	2000000709	-3532027586826254054	1	0
102	2000000710	-7076876310571766028	1	1
102	2000000711	-7871678614102366785	1	0
102	2000000712	-947369421393944280	1	0
102	2000000713	-7249504719250979845	1	0
102	2000000714	5163656879946869082	1	1
102	2000000715	-6689405592075286178	1	0
102	2000000716	2041071870319067295	1	0
102	2000000717	7315194354421844525	1	1
102	2000000718	-1981447961786148634	1	0
102	2000000719	-7761558375622485442	1	2
102	2000000720	2847239473139498991	1	0
102	2000000721	8511227886358174355	1	0
102	2000000722	8663491027335178694	1	0
102	2000000723	-7082171019313824030	1	0
102	2000000724	-6964947218939580574	1	0
102	2000000725	1566535363509039560	1	1
102	2000000726	-5864548989101485226	1	0
102	2000000727	-8884838108438765824	1	0
102	2000000728	-6892007837533608991	1	9
102	2000000729	6640252121598649676	1	0
102	2000000730	6974283897199924901	1	0
102	2000000731	-2413253437294506737	1	0
102	2000000732	-5745046589739276255	1	0
102	2000000733	-2816095407172438235	1	0
102	2000000734	-1526317184543969641	1	0
102	2000000735	-4236279152836641538	1	1
102	2000000736	5883450169777154787	1	0
102	2000000737	-8885949884843028693	1	0
102	2000000738	-4275480352121319611	1	0
102	2000000739	4464399667694645336	1	0
102	2000000740	-8230243735332585566	1	0
102	2000000741	3843506714796885343	1	0
102	2000000742	-6438327568176191801	1	0
102	2000000743	-936893693115527437	1	0
102	2000000744	-8722452410758653635	1	0
102	2000000745	9175510766633525267	1	0
102	2000000746	407113350092855760	1	0
102	2000000747	190569239989277010	1	0
102	2000000748	-1675329261346332849	1	0
102	2000000749	-6997337568353676874	1	0
102	2000000750	5400288476079911027	1	5
102	2000000751	-5094202754092917760	1	0
102	2000000752	-6281351739100713367	1	0
102	2000000753	-8356487029510250283	1	0
102	2000000754	-2768266330783356096	1	0
102	2000000755	2991194027017732164	1	0
102	2000000756	6817250711474100479	1	0
102	2000000757	-8609459946517706479	1	0
102	2000000758	1117597016868852500	1	1
102	2000000759	5480063571633879688	1	0
102	2000000760	-786012976648189977	1	0
102	2000000761	-7867237249420254429	1	0
102	2000000762	78506733819387106	1	0
102	2000000763	7343414712412368710	1	0
102	2000000764	-3616245965538611598	1	2
102	2000000765	9062243209505088646	1	0
102	2000000766	-5589140067204984410	1	0
102	2000000767	-4620830751155899334	1	0
102	2000000768	-4685899739859365940	1	0
102	2000000769	5988353012999221631	1	0
102	2000000770	1625551037688455344	1	1
102	2000000771	4612079968168220183	1	0
102	2000000772	-8903470396850240656	1	0
102	2000000773	7471397317073184649	1	0
102	2000000774	-9073260828057306664	1	0
102	2000000775	-6004689372673379646	1	0
102	2000000776	-638364817117994440	1	1
102	2000000777	-2322817597090801987	1	0
102	2000000778	-6912443921216102974	1	0
102	2000000779	-5206855240369371018	1	0
102	2000000780	3301255963549947634	1	0
102	2000000781	4215354479734997830	1	0
102	2000000782	6581433749055524307	1	0
102	2000000783	5546863820523493479	1	0
102	2000000784	255751341649350614	1	0
102	2000000785	-6627260910814133887	1	0
102	2000000786	1451008053836700258	1	0
102	2000000787	5912273332061696898	1	0
102	2000000788	4914174051134168774	1	1
102	2000000789	-1505864365404196627	1	0
102	2000000790	-727451794362875038	1	0
102	2000000791	2894744210331295187	1	0
102	2000000792	1772480079821854380	1	0
102	2000000793	6535182933820282663	1	0
102	2000000794	5501484454222036205	1	0
102	2000000795	665505556097720381	1	0
102	2000000796	5086047834088109126	1	0
102	2000000797	-1140359889630538607	1	0
102	2000000798	7731012049117049076	1	0
102	2000000799	4038449796716148440	1	0
102	2000000800	-2664297099276952766	1	2
102	2000000801	-2482500879906466549	1	1
102	2000000802	2425597280237009217	1	0
102	2000000803	-586652006490389178	1	0
102	2000000804	6580682854756946552	1	0
102	2000000805	686799993030402996	1	0
102	2000000806	4874361422224701319	1	0
102	2000000807	8502745391283177618	1	0
102	2000000808	8614401276580995037	1	0
102	2000000809	-1327063142942510431	1	0
102	2000000810	1932581422101276016	1	1
102	2000000811	-2291857299264558162	1	0
102	2000000812	-4080222699972974441	1	1
102	2000000813	7720997577829840202	1	0
102	2000000814	3408792733589247099	1	0
102	2000000815	8691023200992283088	1	0
102	2000000816	-242433313724515810	1	1
102	2000000817	2673070882760588525	1	23
102	2000000818	3049192474232067376	1	0
102	2000000819	-2575370679915686837	1	0
102	2000000820	-3076343116555089877	1	0
102	2000000821	5287458118285883886	1	0
102	2000000822	694918346179025770	1	0
102	2000000823	-6342531851170206187	1	0
102	2000000824	-1692105356781825231	1	0
102	2000000825	-992377258616668112	1	0
102	2000000826	-6311014117888634806	1	0
102	2000000827	4848000494819026557	1	0
102	2000000828	1059691320582458907	1	0
102	2000000829	-3032344655395207967	1	0
102	2000000830	-2372598623342294744	1	0
102	2000000831	5425160292901546755	1	0
102	2000000832	76488856091185374	1	1
102	2000000833	1224547687312934289	1	0
102	2000000834	-4155611304333476112	1	0
102	2000000835	-5570697310300715893	1	0
102	2000000836	-4576905349066182435	1	0
102	2000000837	7610556504345267071	1	1
102	2000000838	-7470257043996736661	1	0
102	2000000839	416687423359059407	1	0
102	2000000840	7228942233047580204	1	0
102	2000000841	4546933048618210470	1	0
102	2000000842	-3440651093663044021	1	1
102	2000000843	-292277682007308740	1	0
102	2000000844	4324309647875998771	1	0
102	2000000845	-1178792961506915486	1	0
102	2000000846	-1452112783999086304	1	0
102	2000000847	4208383395991285259	1	0
102	2000000848	2681240363186309918	1	0
102	2000000849	5948516854699542069	1	0
102	2000000850	-8282321649916488413	1	0
102	2000000851	-7870819632915365856	1	2
102	2000000852	7436420335013592145	1	1
102	2000000853	-2047916398335965857	1	0
102	2000000854	-9015728697059075019	1	0
102	2000000855	728247740357176199	1	0
102	2000000856	-9165043103091401601	1	0
102	2000000857	-3020579081032030055	1	0
102	2000000858	4413574436139789906	1	0
102	2000000859	8388470501585506122	1	0
102	2000000860	-4875998285682959509	1	0
102	2000000861	-8011489024666563166	1	0
102	2000000862	4308079095349473247	1	0
102	2000000863	1091469637333404721	1	0
102	2000000864	-4473303655182815444	1	0
102	2000000865	-2275697846518862928	1	1
102	2000000866	-9184799163675668790	1	0
102	2000000867	4057932499337166717	1	0
102	2000000868	-6006606298310346554	1	0
102	2000000869	-7255101302504907991	1	0
102	2000000870	4712272820753174645	1	0
102	2000000871	-7673554198257650506	1	0
102	2000000872	-7262478061143821468	1	0
102	2000000873	5858929025123377530	1	0
102	2000000874	-8351617673807613926	1	0
102	2000000875	-6339998741219588628	1	0
102	2000000876	-1930517508288140553	1	0
102	2000000877	-5004865016691376838	1	0
102	2000000878	-1229892140439693211	1	0
102	2000000879	-7521650831978226364	1	0
102	2000000880	4176625386367073818	1	0
102	2000000881	4794068930792434294	1	0
102	2000000882	-4988149378914569423	1	0
102	2000000883	-6860337138445249149	1	0
102	2000000884	3964857468385941503	1	0
102	2000000885	-4989319812405585052	1	0
102	2000000886	1742266396999405263	1	0
102	2000000887	3886518409372390089	1	0
102	2000000888	-1135443937584918265	1	0
102	2000000889	2006855055502907474	1	0
102	2000000890	-768227824610522298	1	0
102	2000000891	4716354392741012469	1	0
102	2000000892	-3666150636673913382	1	0
102	2000000893	5742032320271158230	1	0
102	2000000894	8039513648741184249	1	0
102	2000000895	-4757149717594373648	1	0
102	2000000896	912800248653254451	1	0
102	2000000897	2171890429284314195	1	0
102	2000000898	6104628488879558047	1	0
102	2000000899	-3467873180011179379	1	0
102	2000000900	-5525605133353891528	1	0
102	2000000901	-6346860121539058337	1	0
102	2000000902	4814594667029517543	1	0
102	2000000903	544509640253020970	1	0
102	2000000904	265829263482519944	1	0
102	2000000905	44681976019897786	1	0
102	2000000906	-5744614909404880627	1	0
102	2000000907	5835430415156872082	1	0
102	2000000908	3465093300269864384	1	0
102	2000000909	-3078742353904403650	1	0
102	2000000910	882326949164834635	1	0
102	2000000911	7664680734285423530	1	0
102	2000000912	883003652861180376	1	0
102	2000000913	-7293576720655050036	1	1
102	2000000914	7923350681241079024	1	0
102	2000000915	-2056343294866171643	1	0
102	2000000916	9081816143670670034	1	0
102	2000000917	-967846679142506426	1	0
102	2000000918	-7204819334085466013	1	0
102	2000000919	5880049292494101918	1	1
102	2000000920	8221084992272232112	1	1
102	2000000921	609604938207610433	1	0
102	2000000922	-347706343151557748	1	0
102	2000000923	-4569641039552417056	1	0
102	2000000924	-4976273116129123078	1	0
102	2000000925	-2625437675942997275	1	0
102	2000000926	7729616558499493031	1	0
102	2000000927	3836076711622384050	1	0
102	2000000928	-8805606788410929812	1	0
102	2000000929	63851718022001911	1	0
102	2000000930	9205696324497013	1	0
102	2000000931	8724533709734202211	1	0
102	2000000932	256728256771641962	1	0
102	2000000933	-1131743231451411598	1	0
102	2000000934	-2449078386832694601	1	0
102	2000000935	6064964023703205388	1	0
102	2000000936	8966706877660743097	1	0
102	2000000937	-4025443318567146955	1	0
102	2000000938	5485868942381942023	1	0
102	2000000939	5879881600104610197	1	0
102	2000000940	-2939408616828985634	1	0
102	2000000941	5406123222108191294	1	0
102	2000000942	4343929996627703905	1	0
102	2000000943	-7672027769739477145	1	0
102	2000000944	7437103956357837389	1	0
102	2000000945	7915545078928575986	1	0
102	2000000946	-8545790135179170598	1	0
102	2000000947	701031601927376825	1	0
102	2000000948	5888195715833119222	1	0
102	2000000949	-9112096851723170186	1	0
102	2000000950	3384374749149532045	1	0
102	2000000951	-327349863373322152	1	0
102	2000000952	-6813084329999632373	1	0
102	2000000953	-7917844593539014293	1	0
102	2000000954	-3560994649842946011	1	0
102	2000000955	2367462802463363422	1	0
102	2000000956	-3613740110692547712	1	0
102	2000000957	6589356139030126645	1	0
102	2000000958	3528086416049257714	1	1
102	2000000959	7071521955825074980	1	0
102	2000000960	4670464998368903761	1	0
102	2000000961	8052128727758468466	1	0
102	2000000962	-4027489574189355398	1	0
102	2000000963	-7772401465520405793	1	0
102	2000000964	-7864230203590923057	1	0
102	2000000965	8910539476105716733	1	0
102	2000000966	-4928145995967954691	1	0
102	2000000967	-986376273584432818	1	1
102	2000000968	-4640904247616785874	1	0
102	2000000969	8190960998390422427	1	0
102	2000000970	-4608901660714913337	1	0
102	2000000971	1275011559545855911	1	0
102	2000000972	-1295201336329561053	1	0
102	2000000973	9091829820384950553	1	1
102	2000000974	6540957371066614316	1	0
102	2000000975	-2019419135082391874	1	0
102	2000000976	-2398904949545213388	1	0
102	2000000977	-9190790590285646942	1	0
102	2000000978	7849459939427238065	1	0
102	2000000979	-8299700317371339995	1	0
102	2000000980	-377606298713362625	1	0
102	2000000981	-2031725293327449026	1	1
102	2000000982	5983264011791497714	1	0
102	2000000983	-7690610360618111555	1	0
102	2000000984	449937141918057277	1	0
102	2000000985	6914732839229375633	1	0
102	2000000986	-8850891524784861637	1	0
102	2000000987	8533968584872633970	1	1
102	2000000988	681146999070498170	1	0
102	2000000989	-3450014328039069239	1	0
102	2000000990	-8342820603299250897	1	0
102	2000000991	-1091438705083176208	1	0
102	2000000992	5902272033214131380	1	1
102	2000000993	7908348201613442623	1	0
102	2000000994	-5149731834386805752	1	0
102	2000000995	-1512542050362921247	1	0
102	2000000996	-2555596903551353834	1	0
102	2000000997	-8591353267454696957	1	0
102	2000000998	4648438223477796593	1	0
102	2000000999	2245323675258877451	1	0
102	2000001000	6959475857629491259	1	0
102	2000001001	-7125449764991703689	1	29
102	2000001002	-2806368011398906520	1	0
102	2000001003	5594314531942316630	1	0
102	2000001004	-8988012340689947584	1	0
102	2000001005	-4528637836425608369	1	0
102	2000001006	-1135189464947323845	1	0
102	2000001007	2885526710536598113	1	1
102	2000001008	1286238108062068368	1	0
102	2000001009	-6648944238338842481	1	0
102	2000001010	6860362571554215216	1	0
102	2000001011	321554501983926968	1	0
102	2000001012	5367931265214041181	1	0
102	2000001013	3538872270517270634	1	0
102	2000001014	7732139213757531631	1	0
102	2000001015	6714215559856081395	1	0
102	2000001016	-7641392127491465904	1	0
102	2000001017	2276997983505728369	1	0
102	2000001018	-1007467999953333188	1	1
102	2000001019	2374965325163934168	1	2
102	2000001020	5616213889778005441	1	3
102	2000001021	8455353873766678633	1	3
102	2000001022	-8398102793415941379	1	2
102	2000001023	-2780071311272975110	1	1
102	2000001024	-8053000820966812681	1	3
102	2000001025	-6664754518424090642	1	6
102	2000001026	-6570008418861600440	1	2
102	2000001027	6551778862476105365	1	13
102	2000001028	6627834638240895512	1	17
102	2000001029	8315107481872866927	1	19
102	2000001030	5670771697453772560	1	0
102	2000001031	1544155196440893856	1	0
102	2000001032	-4770951617551334686	1	1
102	2000001033	-372345773148852929	1	0
102	2000001034	-5733967213181359467	1	2
102	2000001035	-4176918439774775822	1	4
102	2000001036	5691238000848924670	1	1
102	2000001037	-3085202136643791384	1	9
102	2000001038	-6131068197541576738	1	1
102	2000001039	-6862115297513520503	1	1
102	2000001040	-464283600867059024	1	2
102	2000001041	7186828315059264889	1	5
102	2000001042	6158124222323029640	1	2
102	2000001043	-8464602885554311459	1	1
102	2000001044	108747157671611731	1	1
102	2000001045	8462740860802420442	1	1
102	2000001046	-2399654172080959678	1	13
102	2000001047	-8910952457536193267	1	0
102	2000001048	-6906691816955658071	1	1
102	2000001049	-3635710217030240506	1	2
102	2000001050	5857327834671419286	1	8
102	2000001051	-5928123676542923322	1	0
102	2000001052	-1222368133678978019	1	0
102	2000001053	5269657952108192472	1	1
102	2000001054	2067208644914676018	1	7
102	2000001055	-2638291134221394996	1	11
102	2000001056	2153771988474078008	1	4
102	2000001057	-960128883284789234	1	1
102	2000001058	2460692043327998509	1	3
102	2000001059	-367814829649005664	1	9
102	2000001060	1237306865968645953	1	6
102	2000001061	-768449131088247831	1	20
102	2000001062	-891949923407065854	1	3
102	2000001063	-7458424375290572939	1	4
102	2000001064	5898754603752214121	1	3
102	2000001065	6447188493202147399	1	8
102	2000001066	8522311862062046826	1	6
102	2000001067	3009239510960794656	1	8
102	2000001068	4233681671533271443	1	35
102	2000001069	-288608372800445033	1	4
102	2000001070	-1688916975541725591	1	8
102	2000001071	4230669388504180511	1	3
102	2000001072	-1441439608458833047	1	0
102	2000001073	-8693463573350018352	1	0
102	2000001074	-6822581025649830660	1	0
102	2000001075	5299779084982604411	1	13
102	2000001076	7341252202662817654	1	3
102	2000001077	-3734011219728813011	1	3
102	2000001078	-3111047399857813227	1	16
102	2000001079	6356044858478115513	1	5
102	2000001080	7631961285996600547	1	8
102	2000001081	4078814598570768663	1	15
102	2000001082	-2671902171571531285	1	5
102	2000001083	5121651198702756892	1	5
102	2000001084	87388039916460421	1	5
102	2000001085	911759618204730973	1	3
102	2000001086	4291486113038902560	1	5
102	2000001087	-6881348954402300356	1	7
102	2000001088	-6709824203167960709	1	7
102	2000001089	-8128155084156210464	1	4
102	2000001090	-2453704373727120087	1	3
102	2000001091	8257310182211849555	1	4
102	2000001092	2005531911630964251	1	6
102	2000001093	-8131967445052921272	1	4
102	2000001094	-52907678272727815	1	6
102	2000001095	7116507851557313248	1	7
102	2000001096	-7215120721757903283	1	7
102	2000001097	-8960805452828810794	1	3
102	2000001098	-7067785902223299339	1	3
102	2000001099	-1763050638754675372	1	0
102	2000001100	5780901546479079195	1	1
102	2000001101	5524856594142199329	1	1
102	2000001102	3863217587438177010	1	0
102	2000001103	-6530690482946375377	1	1
102	2000001104	5437554715955851851	1	1
102	2000001105	3211889679154096856	1	8
102	2000001106	-2395467479759687223	1	0
102	2000001107	-2573991658597989803	1	0
102	2000001108	-4377735459434946599	1	1
102	2000001109	-72027690974534248	1	0
102	2000001110	207189933907901365	1	0
102	2000001111	2361921548870230688	1	0
102	2000001112	-3252907002280195029	1	3
102	2000001113	4604669814823382222	1	0
102	2000001114	-6435126593051665603	1	4
102	2000001115	7282032306123914706	1	6
102	2000001116	848326468934320527	1	4
102	2000001117	9190602656340832148	1	1
102	2000001118	-6473671706617561918	1	5
102	2000001119	-5726951638775068934	1	1
102	2000001120	-9079328690913641210	1	0
102	2000001121	-9210625787600764149	1	0
102	2000001122	4513977049716890332	1	4
102	2000001123	-9158713616993224031	1	3
102	2000001124	4374512322371477589	1	9
102	2000001125	6357542954624213120	1	3
102	2000001126	7547618722826960891	1	4
102	2000001127	-8143904735275224676	1	1
102	2000001128	-1306018922829213234	1	1
102	2000001129	3842414158465501632	1	2
102	2000001130	3670611775151130911	1	17
102	2000001131	2029903199701682342	1	2
102	2000001132	5516171768802322427	1	1
102	2000001133	8345614292201443968	1	0
102	2000001134	-8879581351573486110	1	0
102	2000001135	8605820832777887285	1	1
102	2000001136	-3966573541100699617	1	1
102	2000001137	3228696117376486889	1	7
102	2000001138	-4996633728173344823	1	0
102	2000001139	6676658122434186560	1	12
102	2000001140	971174339332451099	1	7
102	2000001141	-7177800226208002436	1	0
102	2000001142	307236824694356492	1	0
102	2000001143	2550114325481514518	1	0
102	2000001144	4637543035199056940	1	23
102	2000001145	-6230743295456017513	1	15
102	2000001146	7192667341691898816	1	10
102	2000001147	5205638624216220967	1	3
102	2000001148	-6565075019170510868	1	2
102	2000001149	3782058628936311715	1	1
102	2000001150	-3195672186131227094	1	6
102	2000001151	-5334509675145412683	1	1
102	2000001152	7016315370034764348	1	2
102	2000001153	6476892859771650470	1	0
102	2000001154	-3465740592744057548	1	14
102	2000001155	-3037517196981363286	1	8
102	2000001156	1114369319716674198	1	2
102	2000001157	5751062995806576286	1	8
102	2000001158	236766866536675906	1	5
102	2000001159	-1944632089074046664	1	14
102	2000001160	-7752258273955670937	1	3
102	2000001161	-2928071068988228475	1	3
102	2000001162	4994927038042679865	1	3
102	2000001163	8281097770071769022	1	2
102	2000001164	-2960412080906415157	1	8
102	2000001165	-8762168969870978127	1	2
102	2000001166	2243133368614220796	1	3
102	2000001167	1715240283461579933	1	1
102	2000001168	9219794399687963470	1	2
102	2000001169	-804832200965223130	1	1
102	2000001170	-6364009762563688071	1	7
102	2000001171	1816866295334011362	1	4
102	2000001172	3503386237523232980	1	4
102	2000001173	-2297595275933006673	1	3
102	2000001174	-4392497747760791413	1	4
102	2000001175	-1780397812148861499	1	3
102	2000001176	-3264790370237890194	1	3
102	2000001177	-4578522483913369008	1	0
102	2000001178	-8685499480915168784	1	0
102	2000001179	8991301413776457540	1	1
102	2000001180	7145918418903829447	1	1
102	2000001181	-662693263978555887	1	9
102	2000001182	5179246638415980096	1	7
102	2000001183	364245053286702444	1	282
102	2000001184	-6089330072891960682	1	2
102	2000001185	4974840309390420475	1	0
102	2000001186	-6891234349198819717	1	1
102	2000001187	-1081932165472392682	1	0
102	2000001188	-862757893900596786	1	0
102	2000001189	4761756117214222146	1	0
102	2000001190	-7575432177193452195	1	1
102	2000001191	-4702397548553202652	1	6
102	2000001192	45264677980430591	1	0
102	2000001193	3878448300163266785	1	3
102	2000001194	-9023608126608041850	1	0
102	2000001195	-4416009015091390470	1	0
102	2000001196	7866436034146456517	1	0
102	2000001197	-249521819635086411	1	0
102	2000001198	4501916349915969181	1	0
102	2000001199	2065468963683054448	1	0
102	2000001200	2495145267416411563	1	0
102	2000001201	2015705189481042425	1	0
102	2000001202	-8971579562146055686	1	0
102	2000001203	3703406014009492720	1	0
102	2000001204	7019895460183837475	1	2
102	2000001205	-2813586467317074384	1	0
102	2000001206	-2428216348344006560	1	0
102	2000001207	6553941132847579211	1	0
102	2000001208	-1557936242763417275	1	0
102	2000001209	7643520708485295897	1	0
102	2000001210	7056256915170179218	1	0
102	2000001211	906041612081580848	1	0
102	2000001212	2255057613336132639	1	0
102	2000001213	7239633883044694886	1	0
102	2000001214	3749794823157993345	1	0
102	2000001215	5190530051342802745	1	0
102	2000001216	4822238645918970741	1	0
102	2000001217	4695969812868151224	1	0
102	2000001218	-522817394540157663	1	0
102	2000001219	-2078955599824486635	1	0
102	2000001220	4171638246585978850	1	0
102	2000001221	1813219048352356292	1	0
102	2000001222	4554315221965242666	1	0
102	2000001223	-6833410158936175156	1	0
102	2000001224	-5296972718564795889	1	0
102	2000001225	-1269317059936514769	1	0
102	2000001226	-8468074403158276113	1	0
102	2000001227	7447131582808425736	1	0
102	2000001228	-1332485022344352573	1	0
102	2000001229	5082681135190863564	1	2
102	2000001230	3437836429829108529	1	1
102	2000001231	238092025138455498	1	183
102	2000001232	2849429986525720309	1	1
102	2000001233	1351459559490765744	1	1
102	2000001234	4483650246607868747	1	186
102	2000001235	-2589841752501854624	1	7
102	2000001236	2170570042553337662	1	86
102	2000001237	606864617872302992	1	235
102	2000001238	-2624186850303116539	1	217
102	2000001239	-6857719881763129813	1	0
102	2000001240	-561277564324188454	1	0
102	2000001241	691319980552543269	1	1
102	2000001242	1275445145094723242	1	1
102	2000001243	-3295972363196954376	1	3
102	2000001244	-91721315894732797	1	2
102	2000001245	3092181141534013781	1	0
102	2000001246	3693379232998328949	1	0
102	2000001247	-5922794219681273649	1	0
102	2000001248	1300457086185047770	1	0
102	2000001249	-361911890771219027	1	0
102	2000001250	1925544013814049554	1	0
102	2000001251	2634373055963632102	1	0
102	2000001252	-665776924063090015	1	182
102	2000001253	-5136452061916394765	1	855
102	2000001254	8897796258654925583	1	2656
102	2000001255	-7384987120366478110	1	838
102	2000001256	-5937758128597759100	1	95
102	2000001257	-1416300145659030343	1	2005
102	2000001258	4258774145876756265	1	605
102	2000001259	4223747915072276681	1	605
102	2000001260	-8824255370276270199	1	0
102	2000001261	3177323779712560580	1	0
102	2000001262	3983000563565984390	1	0
102	2000001263	-7000959205335803701	1	0
102	2000001264	-5005933384339086461	1	0
102	2000001265	2698705444242427190	1	181
102	2000001266	687857937432763853	1	0
102	2000001267	-5597116519052833346	1	138
102	2000001268	-2591488856581830582	1	62
102	2000001269	-6544790822615003457	1	470
102	2000001270	-2825558108184742708	1	467
102	2000001271	1168994223344323518	1	22
102	2000001272	-2032319083738454373	1	1
102	2000001273	1796867783899292493	1	1
102	2000001274	-3686252796771842845	1	1
102	2000001275	-2270158545202692171	1	2
102	2000001276	8730914225656170048	1	5
102	2000001277	-8215178805094666448	1	3
102	2000001278	-5291778478237688275	1	0
102	2000001279	1525258728227518028	1	0
102	2000001280	5453959019869486766	1	0
102	2000001281	-7847209498562172930	1	0
102	2000001282	823917457326975110	1	0
102	2000001283	-1036781908657052566	1	0
102	2000001284	7512737960426415814	1	0
102	2000001285	-2550841874831921724	1	0
102	2000001286	-6338598668737218660	1	0
102	2000001287	-327137777713093119	1	0
102	2000001288	9150871031119415570	1	0
102	2000001289	-684804499359966456	1	0
102	2000001290	-6157280172931092825	1	18
102	2000001291	-2737519622774481858	1	5
102	2000001292	1179132302154905766	1	8
102	2000001293	-3143703892714601300	1	5
102	2000001294	-6785070894776717730	1	5
102	2000001295	7748983039484611270	1	4
102	2000001296	4952726493009253463	1	3
102	2000001297	-1852319116449686914	1	3
102	2000001298	-8978907799821498318	1	3
102	2000001299	7959537034108505793	1	3
102	2000001300	-141851571656334866	1	5
102	2000001301	7626764079958247388	1	4
102	2000001302	7836664940262010163	1	4
102	2000001303	4776222105303431085	1	3
102	2000001304	2104993403377692519	1	2
102	2000001305	8524398317037426129	1	2
102	2000001306	-3020259999343376675	1	3
102	2000001307	-6410872747388853356	1	7
102	2000001308	-2734078166033749347	1	3
102	2000001309	696890939607808317	1	3
102	2000001310	-5248779109464195655	1	2
102	2000001311	3635981740501520847	1	3
102	2000001312	7868981472740619149	1	3
102	2000001313	-5382392792071956320	1	3
102	2000001314	4722228480134289451	1	3
102	2000001315	2258045652370874348	1	3
102	2000001316	7458238141391050739	1	3
102	2000001317	5324069311841853375	1	3
102	2000001318	7682350723153265293	1	5
102	2000001319	-255479049949798456	1	8
102	2000001320	8244428772910906594	1	1
102	2000001321	7802840229146774501	1	1
102	2000001322	2672924392723299654	1	0
102	2000001323	3420410409558794600	1	1
102	2000001324	-1879805963535627571	1	1
102	2000001325	7902977540104998009	1	1
102	2000001326	987872124572520775	1	0
102	2000001327	2486188709265545329	1	1
102	2000001328	-7223695806028118107	1	0
102	2000001329	-952303128732388034	1	7
102	2000001330	6220656556765335703	1	5
102	2000001331	-7168421973051444652	1	4
102	2000001332	-7929991792431856222	1	4
102	2000001333	-3009228473796984114	1	7
102	2000001334	7716124063300007096	1	8
102	2000001335	5536513000467519387	1	5
102	2000001336	2628421056183551099	1	4
102	2000001337	-92720985729499244	1	4
102	2000001338	-8226364803093571800	1	4
102	2000001339	-7741358946032307142	1	4
102	2000001340	6920604210740085850	1	6
102	2000001341	-1483961530525751665	1	3
102	2000001342	6213761762685107727	1	1
102	2000001343	-7601845516294979005	1	1
102	2000001344	-8937175801598297559	1	0
102	2000001345	2640566765996170294	1	0
102	2000001346	8144422378637737611	1	0
102	2000001347	8023933319764954538	1	0
102	2000001348	-8310467304190011765	1	0
102	2000001349	5122595439880943005	1	0
102	2000001350	-7367917403558354336	1	1
102	2000001351	8409376315647180451	1	6
102	2000001352	-9085754636720657457	1	0
102	2000001353	-614415171202939901	1	0
102	2000001354	-2457224510879870735	1	0
102	2000001355	-5248650230956437074	1	0
102	2000001356	3638941178979011375	1	0
102	2000001357	9116659678081060652	1	0
102	2000001358	7943931625662181984	1	14
102	2000001359	7765117299931483029	1	186
102	2000001360	-761294447628058446	1	62
102	2000001361	6057698298866825827	1	45
102	2000001362	5222284899394960018	1	46
102	2000001363	5405163203147116641	1	5535
102	2000001364	8702385181061615546	1	9183
\.


--
-- TOC entry 6343 (class 0 OID 17046)
-- Dependencies: 305
-- Data for Name: test_info_archive; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.test_info_archive (build_id, test_id, test_name_id, status, duration) FROM stdin;
\.


--
-- TOC entry 6346 (class 0 OID 17069)
-- Dependencies: 308
-- Data for Name: test_metadata; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.test_metadata (build_id, test_id, test_name_id, key_id, type_id, str_value, num_value) FROM stdin;
\.


--
-- TOC entry 6344 (class 0 OID 17053)
-- Dependencies: 306
-- Data for Name: test_metadata_dict; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.test_metadata_dict (key_id, name_digest, name) FROM stdin;
\.


--
-- TOC entry 6345 (class 0 OID 17062)
-- Dependencies: 307
-- Data for Name: test_metadata_types; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.test_metadata_types (type_id, name) FROM stdin;
\.


--
-- TOC entry 6366 (class 0 OID 17198)
-- Dependencies: 328
-- Data for Name: test_muted; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.test_muted (build_id, test_name_id, mute_id) FROM stdin;
\.


--
-- TOC entry 6315 (class 0 OID 16829)
-- Dependencies: 277
-- Data for Name: test_names; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.test_names (id, test_name, order_num) FROM stdin;
-5168067613992917628	FortitudeTests: FortitudeTests.TestHelpers.TestMetrics.ListProdClassesWithoutMatchingTestClass	1
1658048510990355987	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Trading.ORX.Venues.OrxVenueCriteriaTests.NewVenueCriterias_Serialize_DeserializesProperly	2
395155716920624105	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Trading.ORX.Venues.OrxVenueOrdersTests.NewVenueOrders_Serialize_DeserializesProperly	3
2105868547303546195	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Trading.ORX.Orders.OrxOrderIdTests.NewOrxClient_Serialize_DeserializesProperly	4
4168344068604504176	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Trading.ORX.Orders.OrxOrderTests.NewOrders_Serialize_DeserializesProperly	5
-599379416200460755	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Trading.ORX.Orders.Products.General.OrxSpotOrderTests.NewSpotOrders_Serialize_DeserializesProperly	6
5865893109958142336	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Trading.ORX.Orders.Client.OrxOrderSubmitRequestTests.NewOrderSubmitRequests_Serialize_DeserializesProperly	7
-1014448920728114714	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Trading.ORX.Executions.OrxExecutionsTests.NewVenueOrders_Serialize_DeserializesProperly	8
-5914716010989158113	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Trading.ORX.CounterParties.OrxPartiesTests.NewParties_Serialize_DeserializesProperly	9
4226214848212862725	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Trading.ORX.CounterParties.OrxPartyTests.NewParty_Serialize_DeserializesProperly	10
4142698781433296355	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.EmptyQuote_New_InitializesFieldsAsExpected	11
891115598539416633	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.IntializedFromConstructor_New_InitializesFieldsAsExpected	12
-8247447631318334064	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.NonSourceTickerQuoteInfo_New_ConvertsToSourceTickerQuoteInfo	13
5039604955451317509	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.PopulatedQuote_New_CopiesValuesExceptQuoteInfo	14
320849263338300981	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.NonSourceTickerQuoteInfo_New_CopiesExceptSourceTickerQuoteInfoIsConverted	15
-5488696493139541078	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.EmptyQuote_Mutate_UpdatesFields	16
1097231189897963555	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther	17
-7740681692428656038	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther	18
6553368331669324775	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	19
-8986029802410366237	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.OneDifferenceAtATime_AreEquivalent_ReturnsExpected	20
1605842430492936233	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.PopulatedQuote_GetHashCode_NotEqualToZero	21
104097758823551538	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level0PriceQuoteTests.FullyPopulatedQuote_ToString_ReturnsNameAndValues	22
5914280589009734834	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.EmptyQuote_New_InitializesFieldsAsExpected	23
4213147678250946958	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.IntializedFromConstructor_New_InitializesFieldsAsExpected	24
6183954501267872216	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.NonPeriodSummary_New_ConvertsToPeriodSummary	25
-5205656477440234433	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.PopulatedQuote_New_CopiesValues	26
-7493652208583830495	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.NonSourceTickerQuoteInfo_New_CopiesExceptPeriodSummaryIsConverted	27
2908739203109759186	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.FullyPopulatedQuote_SourceTimeIsGreaterOfBidAskOrOriginalSourceTime	28
5130518625500238005	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.PopulatedQuote_Mutate_UpdatesFields	29
7882878838845453136	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther	30
6892766825010809023	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent	31
-5663271173113375380	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther	32
9154034748061152465	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	33
-1357977716921926776	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.OneDifferenceAtATime_AreEquivalent_ReturnsExpected	34
-6629809875920628607	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.PopulatedQuote_GetHashCode_NotEqualToZero	35
-5312011533091652985	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level1PriceQuoteTests.FullyPopulatedQuote_ToString_ReturnsNameAndValues	36
718348107055543106	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.EmptyQuote_New_InitializesFieldsAsExpected	37
-1157373951113888645	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.IntializedFromConstructor_New_InitializesFieldsAsExpected	38
3837923844096457385	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.NonOrderBooks_New_ConvertsToOrderBook	39
-7864049997018424615	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.SimpleLevel2Quote_New_BuildsOnlyPriceVolumeLayeredBook	40
-2628665152080434732	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.SourceNameLevel2Quote_New_BuildsSourcePriceVolumeLayeredBook	41
7784828775858321386	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.SourceQuoteRefLevel2Quote_New_BuildsSourceQuoteRefPriceVolumeLayeredBook	42
8687322539772689954	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.TraderLevel2Quote_New_BuildsTraderPriceVolumeLayeredBook	43
-8965823899831416089	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.ValueDateLevel2Quote_New_BuildsValueDatePriceVolumeLayeredBook	44
-5014417892241219914	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.EveryLayerLevel2Quote_New_BuildsSourceQuoteRefTraderValueDatePriceVolumeLayeredBook	45
5317262931568349904	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.PopulatedQuote_New_CopiesValues	46
982545823736371990	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.NonOrderBookPopulatedQuote_New_CopiesValuesConvertsOrderBook	47
-6375807517106078134	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.PopulatedQuote_Mutate_UpdatesFields	48
3884006348627733517	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther	49
3773868157680640943	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent	50
4133740822589071545	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther	51
-631540076145704700	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	52
471873135801994195	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.OneDifferenceAtATime_AreEquivalent_ReturnsExpected	53
-8211194052941157511	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.PopulatedQuote_GetHashCode_NotEqualToZero	54
4869087816672658090	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level2PriceQuoteTests.FullyPopulatedQuote_ToString_ReturnsNameAndValues	55
7307247278838092431	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.EmptyQuote_New_InitializesFieldsAsExpected	56
-6754945327394567629	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.IntializedFromConstructor_New_InitializesFieldsAsExpected	57
-7530077675173574284	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.NonRecentlyTraded_New_ConvertsToRecentlyTraded	58
-6813819657005622371	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.NoRecentlyTradedLevel3Quote_New_BuildsOnlyPriceVolumeLayeredBook	59
5013891203746348224	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.SimpleLevel3Quote_New_BuildsOnlySimpleLastTradeEntries	60
8139150691954259558	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.PaidGivenVolumeLevel3Quote_New_BuildsOnlyPaidGivenTradeEntries	61
-6863013941068350251	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.TraderPaidGivenVolumeLevel3Quote_New_BuildsLastTraderPaidGivenEntries	62
8099482474164728756	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.PopulatedQuote_New_CopiesValues	63
3502479247353265537	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.NonRecentlyTradedPopulatedQuote_New_CopiesValuesConvertsRecentlyTraded	64
4421890388133832713	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.PopulatedQuote_Mutate_UpdatesFields	65
-1225547086238709623	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther	66
6004521876572272844	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.FullyPopulatedQuote_CopyFromToEmptyLowerLevelQuote_QuotesLowerIsEquivalent	67
-8579785066513323788	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.PQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther	68
5084515830392146249	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	69
-7631616222065932609	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.OneDifferenceAtATime_AreEquivalent_ReturnsExpected	70
4054285518414936991	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.PopulatedQuote_GetHashCode_NotEqualToZero	71
855622501930378268	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.Level3PriceQuoteTests.FullyPopulatedQuote_ToString_ReturnsNameAndValues	72
4822464996449682968	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.SourceTickerQuoteInfoTests.DummySourceTickerQuoteInfo_New_PropertiesAreAsExpected	73
4804759450788955081	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.SourceTickerQuoteInfoTests.EmptySourceTickerQuoteInfo_New_DefaultAreAsExpected	74
2615522436833709908	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.SourceTickerQuoteInfoTests.DummySourceTickerQuoteInfo_FormatPrice_ReturnsStringFormatterToPrecision	75
-5893936263433988855	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.SourceTickerQuoteInfoTests.When_Cloned_NewButEqualConfigCreated	76
-2806122811888598155	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.SourceTickerQuoteInfoTests.NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame	77
-323908055081131550	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.SourceTickerQuoteInfoTests.OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent	78
-4228791055132281238	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.SourceTickerQuoteInfoTests.PopulatedSti_GetHashCode_NotEqualTo0	79
4585975189901988855	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.SourceTickerQuoteInfoTests.FullyPopulatedSti_ToString_ReturnsNameAndValues	80
-1562883777249695314	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.UniqueSourceTickerIdentifierTests.NewUniqueSourceTickerIdentifer_New_IdGeneratedIsExpected	81
-2261324999257812422	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.UniqueSourceTickerIdentifierTests.GivenASourceIdTickerId_UniqueSourceTickerIdentifier_GeneratesUniqueAndRepeatableKey	82
385756262405517557	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.UniqueSourceTickerIdentifierTests.PopulatedUniqueSourceTickerId_Clone_CreatesNewUniqSourceTickerId	83
-1976503402141874080	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.UniqueSourceTickerIdentifierTests.NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame	84
4525651972397617056	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.UniqueSourceTickerIdentifierTests.OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent	85
828119402709425352	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.UniqueSourceTickerIdentifierTests.PopulatedSti_GetHashCode_NotEqualTo0	86
5598680601370150278	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo.UniqueSourceTickerIdentifierTests.FullyPopulatedSti_ToString_ReturnsNameAndValues	87
-3290616611505052031	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.PQQuoteSerializerRepositoryTests.NewSerializerFactory_CreateQuoteDeserializer_CreatesNewPQQuoteDeserializer	88
-3075720102990604265	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.PQQuoteSerializerRepositoryTests.CreateDeserializer_GetQuoteDeserializer_ReturnsPreviouslyCreatedDeserializer	89
-4205356829618576577	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.PQQuoteSerializerRepositoryTests.NoEnteredDeserializer_GetQuoteDeserializer_ReturnsNullDeserializer	90
1541999056923217352	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.PQQuoteSerializerRepositoryTests.CreateDeserializer_RemoveQuoteDeserializer_RemovesDeserializerAndGetDeserializerReturnsNull	91
-6729386337864249509	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.PQQuoteSerializerRepositoryTests.CreateDeserializer_GetDeserializer_ReturnsDeserializerThatMatchesId	92
-1636241169476163454	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.PQQuoteSerializerRepositoryTests.NoEnteredDeserializer_GetDeserializer_ReturnsDeserializerThatMatchesId	93
-842028361149352176	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.PQQuoteSerializerRepositoryTests.EmptyQuoteSerializationFactory_GetSerializerUintArray_ReturnsPQRequestSerializer	94
-5001296628537403263	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.PQQuoteSerializerRepositoryTests.NoEnteredDeserializer_GetSerializerNonSupportedType_ReturnsDeserializerThatMatchesId	95
-4895682748520660873	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientMessageStreamDecoderTests.TwoQuoteDataUpdates_ProcessTwice_DecodesStreamAndCompletes	96
9207039181192489174	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientMessageStreamDecoderTests.OneQuoteDataUpdateOneHeartbeat_ProcessTwice_DecodesStreamAndCompletes	97
265299714377165253	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientMessageStreamDecoderTests.OneHeartbeatOneQuoteDataUpdate_ProcessTwice_DecodesStreamAndCompletes	98
1850931895478759887	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.NewClientSyncMonitor_New_CreatesStopThreadSignal	99
-3858185599271528368	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.NewClientSyncMonitor_RegisterNewDeserializer_AddsCallbacksToDeserializerAddsDeserializerToKoList	100
5369156447176937088	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.RegisteredKoDeserializer_UnregisterDeserializer_RemovesFromKoList	101
2682672937639856679	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.RegisteredOkDeserializer_UnregisterDeserializer_RemovesFromKoList	102
7644799529120265731	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.TwoDeserializersWithKnownOrder_OnUpdate_SyncProtectsMovesUpdatedDeserializerToEndOfSyncOk	103
546148726491252312	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.UnSyncedSerializer_OnSyncOk_SyncProtectsMovesUpdatedSerializerToEndOfSyncOk	104
-6925810533630939515	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.SyncedDeserializer_OnSyncKo_SyncProtectsMovesDeserializerToFromOkToKo	105
2801260725862711702	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.NewUnstartedClientSyncMonitoring_CheckStartMonitoring_CreateStartsNewBackgroundThread	106
3985171750648239103	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.StartedClientSyncMonitoring_CheckStartMonitoring_DoesNotRestartThread	107
-2094885574236634978	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.StartedClientSyncMonitoring_CheckStopMonitoring_FlagsMonitoringToFinishWaitsForThreadToJoin	108
-7293008259388994363	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.StartedThenStopedClientSyncMonitoring_CheckStopMonitoring_DoesNothing	109
8289558281556610212	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.NewClientSyncMonitoring_MonitorDeserializersForSnapshotResync_RequestsSnapshotValues	110
-8359869987379225846	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.OneIterationTakesTooLong_MonitorDeserializersForSnapshotResync_LogsWarningThatTasksAreSlow	111
4643082334228461166	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.TwoTickersFromSameSource_MonitorDeserializersForSnapshotResync_RequestBothAtSameTime	112
-7182649009264205024	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.UnknownSnapshotServer_MonitorDeserializersForSnapshotResync_SkipsRequestToSendSnapshotRequest	113
-262372815853356321	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.NoTickersRequireSnapshot_MonitorDeserializersForSnapshotResync_SkipsRequestToSendSnapshotRequest	114
2853305180478063174	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.TwoSourcesRequireSnapshoting_MonitorDeserializersForSnapshotResync_SendsTwoRequests	115
273740604063040256	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientSyncMonitoringTests.NewSyncMonitor_MonitorDeserializersForSnapshotResyncReceivesException_CarriesOn	116
5910254892637388891	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClientWith3DispatcherCount_New_Initializes3DispatchersCreated	117
7296555989703615511	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClientWith0DispatcherCount_New_ThrowsInvalidArugment	118
-1647803935715495851	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_GetQuoteStreamNoMulticastLevel0Quote_RegistersAndReturnsQuote	119
-7216378367928879894	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_GetQuoteStreamNoMulticastLevel1Quote_RegistersAndReturnsQuote	120
2642972094844077548	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_GetQuoteStreamNoMulticastLevel2Quote_RegistersAndReturnsQuote	121
6061745905256248282	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_GetQuoteStreamNoMulticastLevel3Quote_RegistersAndReturnsQuote	122
4178047494179091499	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel0QuoteStream_Unsubscribe_RegistersAndReturnsQuote	123
7899792671160188869	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel1QuoteStream_Unsubscribe_RegistersAndReturnsQuote	124
-7946802209278770662	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel2QuoteStream_Unsubscribe_RegistersAndReturnsQuote	125
-2095179500710035246	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel3QuoteStream_Unsubscribe_RegistersAndReturnsQuote	126
-5370593418204654093	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel0Stream_GetQuoteStreamNoMulticastLevel1QuoteSameSourceTicker_ThrowsException	127
5970637498973245149	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel0Stream_GetQuoteStreamNoMulticastLevel1QuoteDiffSourceSameTicker_TwoDiffStreams	128
-5696787878964117539	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel0Stream_GetQuoteStreamNoMulticastLevel1QuoteSameSourceDiffTicker_TwoDiffStreams	129
3886766662433091017	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel0ForTwoDiffCcyStream_Unsubscribe1Stream_LeavesSyncMonitorRunning	130
8292494691086061859	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel0ForTwoDiffCcyStream_Dispose_UnsubscribesBothSubscriptions	131
4985031533047976606	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReturnsNullQueuesInfoRequestAttempt	132
8549850915877672668	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReattemptFindsInfoNoMoreAttemptsAreQueued	133
8229997000000531653	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfo_ReattemptStillDoesntFindInfoMoreAttemptsAreQueued	134
-9128183645133246639	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_GetQuoteStreamNoMultiCastNoFeedInfoThenDispose_UnsubscribeOnReattemptNoInfosRequired	135
7218161957043746000	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_MultipleGetQuoteStreamNoQuoteInfos_OnlyOneReattemptIsQueued	136
-7197118442584039090	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClientTickerNotAvailableInSource_GetQuoteStreamNoMultiCast_ReturnsNullNoQuoteRepoIsQueued	137
2498548561942185946	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel0_GetSourceServerConfig_FindsSnapshotUpdatePricingServerConfig	138
8286629246610762820	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_GetSourceServerConfig_DoesntFindSnapshotUpdatePricingServerConfig	139
-1174808756205235933	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.SubscribedLevel0Stream_RequestSnapshots_FindsSnapshotClientForwardsStreamIdRequests	140
-8304354904086372845	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQClientTests.NewPQClient_RequestSnapshots_DoesntFindSnapshotClientDoesNothing	141
-901036977350955796	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientRegistrationFactoryTests.EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance	142
6165186261284916642	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.MissingSerializationFactory_New_UsesDefaultSerializationFactory	143
5961349759012781409	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.PQSnapshotClient_RequestSnapshots_ConnectsStartsConnectionTimeoutSendRequestIds	144
3358260765246435230	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.PQSnapshotClientNotYetConnected_RequestSnapshots_SchedulesConnectQueuesIdsForSend	145
3541929903117895034	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.AlreadyQueuedIds_RequestSnapshots_LogsIdsAlreadyQueuedForSendOnConnect	146
-1084510063449187158	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.QueuedTickerIdsForRequest_OnConnect_SendsTickerIdsWhenConnected	147
3457924317783329288	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.ConnectedPQSnapshotClient_GetDecoderOnResponse_ResetsDisconnectionTimer	148
8805228457669272158	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.UpdateClient_RecvBufferSize_ReturnsPQClientDecoder	149
5062942869313641847	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.UpdateClient_HasNoStreamToPublisher	150
8881830009048021057	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.ConnectingPQSnapshotClient_TimeoutConnection_CallsDisconnect	151
6762983883462077005	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.DefaultFactoryPQSnapshotClientStreamToPublisher_New_RegistersPublisherForMessageId0	152
-5747231119492549359	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSnapshotClientTests.PQSnapshotClientStreamToPublisher_SendBufferSize_StreamFromSubscriber_AreExpected	153
3222081366833223268	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSocketSubscriptionRegistrationFactoryBaseTests.EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance	154
2466633683436661857	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSocketSubscriptionRegistrationFactoryBaseTests.EmptySocketSubRegFactory_FindSocketSubscription_ReturnsNull	155
-1575323095389685462	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSocketSubscriptionRegistrationFactoryBaseTests.RegisteredSocketSubscriber_UnregisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance	156
-802959555982311160	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQSocketSubscriptionRegistrationFactoryBaseTests.MultiRegisteredFactory_UnregisterSocketSubscriber_DoesntRemoveSubscriptionUntilAllUnsubscribed	157
-2304382874982447364	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionQuoteStreamTests.NewTickerFeedSubscription_MultiipleSubscribe_AddsObserverToSubscription	158
-7215277425977975246	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionQuoteStreamTests.NewTickerFeedSubscription_MultipleSubscribeThenOneUnsubscribe_AddsObserverToSubscription	159
-1094041455508475148	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionQuoteStreamTests.NewTickerFeedSubscription_OnComplete_CallUncompleteOnObserver	160
3421573324132374923	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionQuoteStreamTests.NewTickerFeedSubscription_OnError_CallUncompleteOnObserver	161
-9207514556493929158	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionQuoteStreamTests.AddedCleanupActions_Unsubscribe_RunsCleanupActions	162
4504696974646285517	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionQuoteStreamTests.AddedCleanupActionsBeforeAnySubscriptions_LastSubscriptionDisposed_RunsCleanupActions	163
5780058753660829978	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionQuoteStreamTests.AddedCleanupActionsAfterAnySubscriptions_LastSubscriptionDisposed_RunsCleanupActions	164
5788044238413357566	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionQuoteStreamTests.AddingNewObserver_Subscribe_ProtectsObserverCollectionInQuoteSyncLock	165
45366133087923521	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionQuoteStreamTests.RemovingSubscriber_SubscriberDispose_ProtectsObserverCollectionInQuoteSyncLock	166
-3107720997913478708	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQTickerFeedSubscriptionTests.NewTickerFeedSubscription_Properties_InitializedAsExpected	167
-7414640722507565204	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQUpdateClientRegistrationFactoryTests.EmptySocketSubRegFactory_RegisterSocketSubscriber_FindSocketSubscriptionReturnsSameInstance	168
2728663142375709036	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQUpdateClientTests.MissingSerializationFactory_New_UsesDefaultSerializationFactory	169
5532191644254049702	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQUpdateClientTests.UpdateClient_GetDecoder_ReturnsPQClientDecoder	170
-2575506855031989672	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQUpdateClientTests.UpdateClient_RecvBufferSize_ReturnsPQClientDecoder	171
3913174321725019164	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Subscription.PQUpdateClientTests.UpdateClient_HasNoStreamToPublisher	172
-6325375061704290982	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQHeartbeatSerializerTests.TwoQuotesBatchToHeartBeat_Serialize_SetTheExpectedBytesToBuffer	173
1952829678854231830	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQHeartbeatSerializerTests.FullBuffer_Serialize_WritesNothingReturnsNegativeWrittenBytes	174
-7714358181202295009	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQHeartbeatSerializerTests.AlmostFullBuffer_Serialize_WritesHeaderReturnsNegativeWrittenAmount	175
167381447430374963	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQQuoteSerializerTests.UpdateSerializerFullyUpdated_Serialize_WritesExpectedBytesToBuffer	176
-7156862266043976418	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQQuoteSerializerTests.SnapshotSerializerNoUpdates_Serialize_WritesAllDataExpectedBytesToBuffer	177
7453602060532121344	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQQuoteSerializerTests.UpdateSerializerSaveToBuffer_Deserializer_CreatesEqualObjects	178
2048672468661446044	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQServerSerializationRepositoryTests.NewSerializationFactory_GetSerializer_ReturnsAppropriateSerializerForMessageType	179
-5313316152668899945	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQServerSerializationRepositoryTests.QuoteSerializerWrongId_GetSerializerWrongIdsForType_ThrowsNotSupportedException	180
6546417840909422353	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQServerSerializationRepositoryTests.HeartBeatSerializerWrongId_GetSerializerWrongIdsForType_ThrowsNotSupportedException	181
7998291862378205807	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.PQSnapshotIdsRequestSerializerTests.ListOfSnapshotIds_Serialize_WritesExpectBytesToBuffer	182
1583629675819367816	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.NewPQQuoteDeserializer_New_SetsSourceTickerIdentifer	183
6015330456467304167	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.RegisteredCallback_InvokeOnReceivedUpdate_CallsCallback	184
-3922533019978291516	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.RegisteredCallback_InvokeOnSyncOk_CallsCallback	185
6469926083620759826	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.RegisteredCallback_InvokeOnOutOfSync_CallsCallback	186
-6197704211498601688	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.NewPQQuoteDeserializer_Subscribe_SyncLockProtectsAddingObserver	187
7506018132291755046	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.SubscribedObserver_DisposeSubscription_SyncLockProtectsRemovingObserver	188
-2854098181133654390	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.EmptyQuoteLvl0Quote_UpdateQuote_SetsDispatcherContextDetails	189
5580666055292867697	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.EmptyQuoteLvl1Quote_UpdateQuote_SetsDispatcherContextDetails	190
-5586930227402413306	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.EmptyQuoteLvl2Quote_UpdateQuote_SetsDispatcherContextDetails	191
-5863135281082852428	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.EmptyQuoteLvl3Quote_UpdateQuote_SetsDispatcherContextDetails	192
6876094986674014987	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.SubscribedTickerQuoteHasChanges_PushQuoteToSubscribers_LatencyTraceSyncLocksUpdatedQuote	193
-4387068365724119821	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.NoSubscribedObservers_PushQuoteToSubscribers_ReturnsDoesNothing	194
7685242590091901238	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQDeserializerBaseTests.SubscribersNoQuoteupdates_PushQuoteToSubscribers_SetsPubStatusDoesntPush	195
-2846826406673728752	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.FreshSerializer_DeserializeSnapshot_SyncClientQuoteWithExpected	196
9174150359974885957	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.FreshSerializer_DeserializeFullUpdateSequenceId0_SyncClientQuoteWithExpected	197
-2872144303876561253	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.FreshSerializerMissedUpdates_DeserializeManyUnexpectedSeqId_StaysUnsync	198
-3668471866226188888	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.FreshSerializerMissedUpdates_DeserializeManyUnexpectedSeqIdThenSyncSnapshot_PublishesLatestQuote	199
2286211212203948379	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.InSyncDeserializer_DeserializeOutofOrderUpdateThenMissingIdUpdate_GoesOutOfSyncThenInSyncAgain	200
3876868695864254709	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.InSyncDeserializer_DeserializeOutofOrderUpdateThenHigherSnapshotId_GoesOutOfSyncThenInSyncAgain	201
-8982279052801127019	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.InSyncDeserializer_Timesout_PublishesTimeoutState	202
-1191290804749559924	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.TimedOutDeserializer_ReceivesNextUpdate_GoesBackToInSync	203
3188356211687288195	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.TimedOutDeserializer_HasTimedOutAndNeedsSnapshot_ReturnsFalseNeedsSnapshot	204
8158340901452721225	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.OutOfSyncDeserializer_RequestSnapshotReceivesUpdatesUpToBuffer_GoesInSyncPublishesLatestUpdate	205
6159620885790173160	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.OutOfSyncDeserializer_RequestSnapshotReceivesUpdatesMoreThanBuffer_PublishesNothing	206
3132744306913834556	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteDeserializerTests.SynchronisingDeserializer_CheckResyncAfterWaitTimeout_ReturnsTrueToStartSynchronisationRequest	207
-3207103653603210855	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.PQQuoteFeedDeserializerTests.FreshSerializer_DeserializeSnapshot_SyncClientQuoteWithExpected	208
-8222704019872743726	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.DeserializeStateTransitionFactoryTests.NewTransitionFactory_TransitionToState_MovesThroughAllPossibleStates	209
-8768095674480220028	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.DeserializeStateTransitionFactoryTests.ExistingRetrievedState_TransitionToState_ReturnsSameInstance	210
4768003828651437716	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.DeserializeStateTransitionFactoryTests.NewTransitionFactory_TransitionToStateInitialization_ThrowsException	211
8906314032266130370	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InitializationStateTests.NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour	212
-7100702980283025836	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InitializationStateTests.NewSyncState_ProcessInStateProcessNextExpectedUpdateCantSync_LogsProblem	213
1344233066732476800	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InitializationStateTests.NewSyncState_ProcessSnapshot_CallsExpectedBehaviour	214
7863800826459049259	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InitializationStateTests.NewSyncState_ProcessInStateProcessSnapshotCantSync_LogsProblem	215
-5635970143258691064	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InitializationStateTests.NewSyncState_EligibleForResync_ReturnsExpected	216
399603175506464863	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InitializationStateTests.NewSyncState_ProcessInStateProcessSnapshotMovesToSnapshot_LogsRecovery	217
-4159700018826741568	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InitializationStateTests.NewSyncState_LinkedDeserializerAndState_SetAsExpected	218
-1217956621720234165	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InitializationStateTests.NewSyncState_HasJustTimedOut_ReturnsExpected	219
-4379048972556788231	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InSyncStateTests.NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour	220
6910590723172147604	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InSyncStateTests.NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour	221
-5436223351934266950	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InSyncStateTests.NewSyncState_HasJustGoneStale_CallsExpectedBehaviour	222
3385906740431711903	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InSyncStateTests.NewSyncState_LinkedDeserializerAndState_SetAsExpected	223
1015688014506287140	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InSyncStateTests.NewSyncState_EligibleForResync_ReturnsExpected	224
-4545065987677879872	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InSyncStateTests.NewSyncState_HasJustTimedOut_ReturnsExpected	225
-1690653735689708521	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.InSyncStateTests.NewSyncState_ProcessSnapshot_CallsExpectedBehaviour	226
9152719559837512697	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.StaleStateTests.NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour	227
2112934594205575010	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.StaleStateTests.NewSyncState_HasJustGoneStale_CallsExpectedBehaviour	228
-6921788027242763511	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.StaleStateTests.NewSyncState_ProcessUnsyncedUpdateMessage_CallsExpectedBehaviour	229
9026668089164890926	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.StaleStateTests.NewSyncState_LinkedDeserializerAndState_SetAsExpected	230
7835985019669643007	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.StaleStateTests.NewSyncState_EligibleForResync_ReturnsExpected	231
1748930001085728374	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.StaleStateTests.NewSyncState_HasJustTimedOut_ReturnsExpected	232
-4870757489143658629	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.StaleStateTests.NewSyncState_ProcessSnapshot_CallsExpectedBehaviour	233
-8946682362426085462	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SynchronisingStateTests.NewSyncState_EligibleForResync_ReturnsExpected	234
6482940291938471003	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SynchronisingStateTests.NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour	235
-5100029622945035865	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SynchronisingStateTests.NewSyncState_ProcessInStateProcessNextExpectedUpdateCantSync_LogsProblem	236
5861494255440031174	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SynchronisingStateTests.NewSyncState_ProcessSnapshot_CallsExpectedBehaviour	237
-3704034302743523421	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SynchronisingStateTests.NewSyncState_ProcessInStateProcessSnapshotMovesToSnapshot_LogsRecovery	238
-3910353358171428391	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SynchronisingStateTests.NewSyncState_LinkedDeserializerAndState_SetAsExpected	239
7178471542052155939	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SynchronisingStateTests.NewSyncState_HasJustTimedOut_ReturnsExpected	240
-4846440185093465276	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SyncStateBaseTests.NewSyncState_LinkedDeserializerAndState_SetAsExpected	241
-5098932985785461169	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SyncStateBaseTests.NewSyncState_EligibleForResync_ReturnsExpected	242
3253163512775453786	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SyncStateBaseTests.NewSyncState_HasJustTimedOut_ReturnsExpected	243
-2790709064176103988	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SyncStateBaseTests.NewSyncState_ProcessInStateProcessNextExpectedUpdate_CallsExpectedBehaviour	244
871906643331371361	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState.SyncStateBaseTests.NewSyncState_ProcessSnapshot_CallsExpectedBehaviour	245
-4909190956136147169	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQImplementationFactoryTests.NewPQImplementationFactory_GetConcreteMapping_GetsConcreateImplementationOfInterface	246
-811238283709010921	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQImplementationFactoryTests.NonSupportedPQType_GetConcreteMapping_GetsConcreateImplementationOfInterface	247
-3044470657175993418	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.EmptyQuote_SourceTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	248
7882824793751932003	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.EmptyQuote_SyncStatusChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	249
7742639209789501280	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.EmptyQuote_SingPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	250
-2046409902734848786	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.EmptyQuote_ReplayChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	251
-5682229402934137803	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.EmptyQuote_FieldsSetThenResetFields_SameEmptyQuoteEquivalent	252
-6243293462249496788	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel0Fields	253
8170965163082980576	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel0Fields	254
-1626235145649623620	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields	255
854370132355022024	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.PopulatedQuote_GetDeltaUpdatesUpdatePersistenceThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote	256
3273353310598347926	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote	257
8901378153430543944	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerQuoteInfo	258
-6177029893441046382	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateField_ReturnsSizeFoundInField	259
-7480954548352835387	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues	260
-3716360097816402166	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther	261
-1514631603694905049	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	262
5140311844596731374	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther	263
671267312631720891	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal	264
-8631849075709006578	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	265
3441965872625483974	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.FullyPopulatedQuoteSameObj_Equals_ReturnsTrue	266
-5685808563646212420	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel0QuoteTests.EmptyQuote_GetHashCode_ReturnNumberNoException	267
-5859128146916308836	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.FullyPopulatedQuote_SourceTimeIsGreaterOfBidAskOrOriginalSourceTime	268
42973866600002177	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.EmptyQuote_SourceAskTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	269
3442892229187295721	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.EmptyQuote_SourceBidTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	270
-8635253897124206828	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.EmptyQuote_AdapterSentTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	271
5460041067622205790	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.EmptyQuote_AdapterReceivedTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	272
8068526363439912750	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.EmptyQuote_BidPriceTopChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	273
-3276954727298532722	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.EmptyQuote_AskPriceTopChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	274
-3844892461784491471	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.EmptyQuote_ExecutableChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	275
8984070560824590680	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.EmptyQuote_FieldsSetThenResetFields_SameEmptyQuoteEquivalent	276
-6791269373554175731	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel1Fields	277
5413119706348108665	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel1Fields	278
-5649409396766215112	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	279
-2956507839275432988	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote	280
6304524793301695953	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther	281
-2531294556859143024	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	282
-3591361680031440861	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther	283
-4726818166408084722	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal	284
9160233208864691179	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	285
8439687064434765458	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.FullyPopulatedQuoteSameObj_Equals_ReturnsTrue	286
333483086096992788	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel1QuoteTests.EmptyQuote_GetHashCode_ReturnNumberNoException	287
-2294112922519809294	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.TooLargeMaxBookDepth_New_CapsBookDepthTo	288
-1782386555252545714	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.TooSmalMaxBookDepth_New_IncreaseBookDepthAtLeast1Level	289
-2319932335302408233	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.SimpleLevel2Quote_New_BuildsOnlyPriceVolumeLayeredBook	290
-6941344528592049593	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.SourceNameLevel2Quote_New_BuildsSourcePriceVolumeLayeredBook	291
-8387929219434347608	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.SourceQuoteRefLevel2Quote_New_BuildsSourceQuoteRefPriceVolumeLayeredBook	292
5534209550501235227	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.TraderLevel2Quote_New_BuildsTraderPriceVolumeLayeredBook	293
-7842144647537605462	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.ValueDateLevel2Quote_New_BuildsValueDatePriceVolumeLayeredBook	294
3529563026656807306	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.EveryLayerLevel2Quote_New_BuildsSourceQuoteRefTraderValueDatePriceVolumeLayeredBook	295
3292015599141719441	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_IsBookChanged_SetsAndReadsBookChangedStatus	296
-1956624619424199379	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_IsHasUpdates_SetsAndReadsBookChangedStatus	297
2216113875507654384	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.SimpleLevel2Quote_OrderBookViaInterfaces_RetrievesSameInstance	298
-6962178826515084706	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.SimpleLevel2Quote_MutableSetOrderBook_OnlyAllowsPQOrderBookToBeSet	299
8840354425581028533	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.SimpleLevel2Quote_AskPriceTop_SameAsBookLevel0	300
-4659232289303006199	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.SimpleLevel2Quote_BidPriceTop_SameAsBookLevel0	301
2893223015212878224	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_LayerPriceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	302
500549125022718037	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_LayerVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	303
1596753244653843244	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_LayerSourceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	304
2317779630936083971	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_LayerExecutableChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	305
-4790867754608461845	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_LayerSourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	306
8355975946991213234	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_LayerValueDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	307
4525469924466266750	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_LayerTraderCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	308
8339961779989768797	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_LayerTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	309
6768425885042804800	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.AllLevel2QuoteTypes_LayerTraderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	310
-3702124768431439213	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel2Fields	311
-2151332099485546499	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.TraderPopulatedWithUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel2Fields	312
6805988862252946054	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel2Fields	313
4186957571204407323	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.TraderPopulatedWithUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel2Fields	314
-1137552624966346092	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	315
-6843401644120111781	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote	316
-4775450474354578645	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther	317
673073708176228709	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	318
-6042258967121502890	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther	319
5454844165586025251	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal	320
4781022324913180018	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	321
7198195242312020334	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.FullyPopulatedQuoteSameObj_Equals_ReturnsTrue	322
8061551208747540058	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel2QuoteTests.FullyPopulatedQuote_GetHashCode_ReturnNumberNoException	323
975291638248038318	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.NoRecentlyTradedLevel3Quote_New_BuildsOnlyPriceVolumeLayeredBook	324
5296186274952516017	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.SimpleLevel3Quote_New_BuildsOnlySimpleLastTradeEntries	325
-7220900720455481494	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.PaidGivenVolumeLevel3Quote_New_BuildsOnlyPaidGivenTradeEntries	326
4598769653297822368	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.TraderPaidGivenVolumeLevel3Quote_New_BuildsLastTraderPaidGivenEntries	327
5557147014809399260	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.EmptyLevel3Quote_BatchIdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	328
-6300936569192378386	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.EmptyLevel3Quote_SourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	329
6583713035894075762	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.EmptyLevel3Quote_ValueDateChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	330
-7148742707063303148	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.AllLevel3QuoteTypes_LastTradePriceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	331
6891133056092466157	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.AllLevel3QuoteTypes_LastTradeTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	332
8847504368228341853	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.AllLevel3QuoteTypes_LastTradeWasGivenChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	333
-769850871404066714	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.AllLevel3QuoteTypes_LastTradeWasPaidChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	334
5888195715833119222	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.EmptyEntry_Mutate_UpdatesFields	949
8010487579726324572	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.AllLevel3QuoteTypes_LastTradeVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	335
-1281815806852441397	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.AllLevel3QuoteTypes_LastTradeTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	336
1938521020609095059	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.AllFullyPopulatedQuotes_HasUpdatesSetFalse_RemovesUpdatesFromAllLastTrades	337
3523884679671725957	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.AllFullyPopulatedQuotes_Reset_SameAsEmptyQuotes	338
810398108306760348	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel3Fields	339
-3753456124963804734	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel3Fields	340
9055249480909265145	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	341
1376848864724890411	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.PopulatedQuote_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote	342
-7249976424530137073	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.FullyPopulatedQuote_CopyFromToEmptyQuote_QuotesEqualEachOther	343
4964813406853494573	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.FullyPopulatedQuote_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	344
-4591010665574936763	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.NonPQPopulatedQuote_CopyFromToEmptyQuote_QuotesEquivalentToEachOther	345
287894396417678741	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal	346
-3260839014246629641	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	347
4089557322633378077	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.FullyPopulatedQuoteSameObj_Equals_ReturnsTrue	348
-37434540835908810	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.PQLevel3QuoteTests.FullyPopulatedQuote_GetHashCode_ReturnNumberNoException	349
-6012374541873459228	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuoteInfo_RoundingPrecisionChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	350
-8900759627960524397	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuoteInfo_MinSubmitSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	351
-6743800908559635352	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuoteInfo_MaxSubmitSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	352
8199428507212141948	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuoteInfo_IncrementSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	353
-7201485080435899140	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuoteInfo_MinimumQuoteLifeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	354
1921674646476416089	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuoteInfo_LayerFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	355
-1963231258293348397	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuoteInfo_MaximumPublishedLayersChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	356
-8218325220138309565	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuoteInfo_LastTradedFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	357
-5848613183465042385	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.PopulatedQuoteInfo_FormatPrice_Returns0MatchingNumberOfDecimalPlaces	358
7878477018103303313	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.PopulatedQuoteInfo_GetDeltaUpdateFieldsAsUpdate_ReturnsAllQuoteInfoFields	359
-652171486573031821	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllQuoteInfoFields	360
-6985289698676835606	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields	361
-5041753597538503662	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.PopulatedQuote_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote	362
3530066322638971749	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerQuoteInfo	363
-2370561505487284775	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateField_ReturnsSizeFoundInField	364
1672842826120172223	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuoteInfo_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues	365
-2389231846021142751	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.FullyPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualEachOther	366
-5707071873651388052	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.NonPQPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualToEachOther	367
5504638158992425192	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal	368
-3103166860339059077	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	369
-8762091848778466957	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.FullyPopulatedQuoteSameObj_Equals_ReturnsTrue	370
-2138427480696216105	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQSourceTickerQuoteInfoTests.EmptyQuote_GetHashCode_ReturnNumberNoException	371
-8727409099088109660	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.EmptyQuoteInfo_IdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	372
-5455537310022155887	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.EmptyQuoteInfo_SourceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	373
-167802844271584258	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.EmptyQuoteInfo_TickerChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	374
2198556615269313949	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.PopulatedQuoteInfo_GetDeltaUpdateFieldsAsUpdate_ReturnsAllQuoteInfoFields	375
7040673462681810840	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllQuoteInfoFields	376
123339232929051273	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields	377
-3971936715945561297	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.PopulatedQuote_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote	378
-5889541757896086778	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerQuoteInfo	379
7380966007935825479	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateField_ReturnsSizeFoundInField	380
5345430275619616017	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.EmptyQuoteInfo_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues	381
-3268371542770224896	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.FullyPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualEachOther	382
-9150411862071801368	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.NonPQPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualToEachOther	383
-8627233018830102918	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal	384
2383175652925770064	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.TwoFullyPopulatedIds_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	385
-3529455259661086276	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.FullyPopulatedQuoteSameObj_Equals_ReturnsTrue	386
8112924697463551049	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo.PQUniqueSourceTickerIdentifierTests.EmptyQuote_GetHashCode_ReturnNumberNoException	387
-3889480022973573131	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQPublisherTests.NewPQPublisher_RegisterTickersWithServer_WaitsToServerStartToRegister	388
-554800153383183542	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQPublisherTests.ConfiguredTicker_PublishReset_PushesEmptyPictureWithUpdatedTimestampsToServer	389
3174605870365863445	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQPublisherTests.TickerWithValues_PublishQuoteUpdate_CopysQuoteDetailsTo	390
-6010461173274098506	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQPublisherTests.TickerWithValues_Dispose_PublishesResetToAllTickers	391
3972174041477632771	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerHeartBeatSenderTests.NewHeartBeatSender_StartServices_LaunchesNewThreadAsBackground	392
-599618953858237035	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerHeartBeatSenderTests.StartedHeartBeatSender_StopAndWaitUntilFinished_WaitsForBackgroundThreadToFinish	393
-3332655978695007023	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerHeartBeatSenderTests.StartedHeartBeatSender_CheckPublishHeartBeats_PublishesHeartBeatToTwoQuotesOverTolerance	394
3334900419860551377	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerHeartBeatSenderTests.StartedHeartBeatSenderAllQuotesJustPublished_CheckPublishHeartBeats_PublishesNothing	395
-8391786289264016656	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerHeartBeatSenderTests.StartedHeartBeatSenderDisconnectedUpdateServer_CheckPublishHeartBeats_PublishesNothing	396
2324740851889752724	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerHeartBeatSenderTests.StartedHeartBeatSender_CheckPublishHeartBeats_ProtectsUpdatesToQuotesList	397
-1306535233403231905	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerHeartBeatSenderTests.StartedHeartBeatSender_CheckPublishHeartBeats_RecoversAfterExceptionToPublish	398
3691910877796480479	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerHeartBeatSenderTests.StartedHeartBeatSenderJustChecked_CheckPublishHeartBeats_PausesUntilNextCheckTime	399
4101574638442679856	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerMessageStreamDecoderTests.NewServerDecoder_New_PropertiesInitializedAsExpected	400
2528460505267493704	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerMessageStreamDecoderTests.TwoSnapshotRequests_ProcessTwice_DecodesStreamAndCompletes	401
3868823698102365907	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.NewPQServer_StartServices_ConnectsUpdateServiceAndListensToSnapshotService	402
1210847126746082376	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.NewPQServer_StartServices_GivesHeartBeatServiceUpdateService	403
6274690045188883515	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.AlreadyStartedPQServer_StartServices_IgnoresRequest	404
2448275589862489061	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.PQServerWithNullQuoteFactory_Register_CreateLevel0QuoteReadyForPublish	405
1019708284290908756	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.PQServerWithNullQuoteFactory_Register_CreateLevel1QuoteReadyForPublish	406
5731224940909260024	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.PQServerWithNullQuoteFactory_Register_CreateLevel2QuoteReadyForPublish	407
-4461578282072854891	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.PQServerWithNullQuoteFactory_Register_CreateLevel3QuoteReadyForPublish	408
4422570313003487721	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.StartedPQServer_Register_PublishesEmptyQuoteToConnectedUpdateServer	409
-6975057738140130921	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.StartedPQServer_Register_DoesNotPublishedToUnconnectedUpdateServer	410
2583041290795623063	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.StartedPQServer_Register_StartsUnStartedHeartBeatSender	411
4554372258448489687	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.StartedPQServerHeartBeatingAlready_Register_LeavesHeartBeatSenderAlone	412
4676693325818911216	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.StartedPQServer_RegisterUnknownTicker_ReturnsNoQuoteForPublishing	413
-6899431719471549278	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.RegisteredPQServer_Publish_ProtectsUpdatesToQuoteBehindQuoteSyncLock	414
3509701785538232647	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.RegisteredPQServer_Publish_ReordersPublishedQuoteToEndOfHeartBeats	415
-9072473139632003956	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.RegisteredPQServer_Publish_ProtectsReorderedHeartBeat	416
-8218772127320035485	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.PQServer_PublishUnregisteredQuote_ExpectArgumentException	417
-1340594121368301723	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.RegisteredPQServerDisconnectedUpdateServer_Publish_DoesNotSendReconnectDoesSend	418
-6617398307302016737	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.StartedRegisteredPQServer_SnapshotRequestReceived_SendsSnapshotsForEachIdRequested	419
7377611215149686245	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.RegisteredNonEmptyQuotePQServer_Unregister_PublishesEmptyQuote	420
-8717890372998608465	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.RegisteredNonEmptyQuotePQServer_Unregister_RemovesRegisteredQuote	421
-7883477971508375745	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ReturnsSamePQPriceVolumeLayerType	631
7861568514571853282	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.NonPQPriceVolumeLayerTypes_TypeCanWholeyContain_ReturnsAsExpected	632
3976811048074606695	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.PQPriceVolumeLayerTypes_TypeCanWholeyContain_ReturnsAsExpected	633
-7657782823826474449	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.NonPQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToSrcQtRefTrdrVlDtPVLIfCantContain	634
8033395148388329747	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToSrcQtRefTrdrVlDtPVLIfCantContain	635
7521579517602025726	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.NullPriceVolumeLayerEntries_SelectPriceVolumeLayer_HandlesEmptyValues	636
5133758865275727318	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PriceVolumeLayerFactoryTests.NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected	637
353410713350147686	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PriceVolumeLayerFactoryTests.InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields	638
4370706321304864293	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.SourcePriceVolumeLayerFactoryTests.NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected	639
-604599232154294899	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.SourcePriceVolumeLayerFactoryTests.InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields	640
4200379726516780142	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.SourceQuoteRefPriceVolumeLayerFactoryTests.NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected	641
-4856317104683875529	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.SourceQuoteRefPriceVolumeLayerFactoryTests.InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields	642
2948843064339980442	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.SourceQuoteRefTraderValueDatePriceVolumeLayerFactoryTests.NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected	643
5226501621194592299	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.SourceQuoteRefTraderValueDatePriceVolumeLayerFactoryTests.InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields	644
-3749193557444209145	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.TraderPriceVolumeLayerFactoryTests.NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected	645
-3068262110343687907	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.TraderPriceVolumeLayerFactoryTests.InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields	646
-7371290191581905607	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.ValueDatePriceVolumeLayerFactoryTests.NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected	647
-1497134584702649418	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.ValueDatePriceVolumeLayerFactoryTests.InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields	648
-3768139355512319682	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.LastRegisteredQuote_Unregister_StopsStartedHeartBeatingSender	422
186989653214630812	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.StartedPublishingPQServer_Dispose_StopsUpdateAndSnapshotServerAndDiscards	423
-8575991715294204431	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQServerTests.StartedPublishingPQServer_Dispose_SyncProtectsClearingThenStopsHeartBeatService	424
-2135398279124016704	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQSnapshotServerTests.NewPQSnapShotServer_RegisterSerializer_ForPQFullSnapshot	425
3745991302157209440	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQSnapshotServerTests.NewPQSnapshotServer_GetFactory_ReturnsSerializationFactory	426
7200778840955335664	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQSnapshotServerTests.NewPQSnapshotServer_SendBufferSize_ReturnsExpectedSize	427
5403114043803834687	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQSnapshotServerTests.NewPQSnapshotServer_SnapshotClientStreamFromSubscriber_CanListenForSnapshotRequests	428
7471078443531567468	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQSnapshotServerTests.NewPQSnapshotServer_SnapshotClientStreamFromSubscriber_RecvBufferIsExpectedSize	429
-6516337435024933937	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQSnapshotServerTests.StreamFromSubscriber_StreamToPublisher_RefersBackToReferencingPublisher	430
-8157418118392356068	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQSnapshotServerTests.StreamFromSubscriber_OnRecvZeroBytes_DoesNotCloseSocket	431
-8941669507633053478	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQUpdatePublisherTests.NewPQSnapShotServer_RegisterSerializer_ForPQFullSnapshot	432
1750220706064829111	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQUpdatePublisherTests.NewPQUpdateServer_GetFactory_ReturnsSerializationFactory	433
-323380473992954954	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQUpdatePublisherTests.NewPQSnapshotServer_SendBufferSize_ReturnsExpectedSize	434
-4750834435677326447	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication.PQUpdatePublisherTests.StreamFromSubscriber_OnRecvZeroBytes_DoesNotCloseSocket	435
6395554917994421796	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FromSourceTickerQuoteInfo_New_InitializesOrderBookWithExpectedLayerTypes	436
-2393752537121269767	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.NonPQLayers_New_ConvertsToPQEquivalent	437
-6232339777137704434	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PQOrderBook_InitializedFromOrderBook_ConvertsLayers	438
-520295747944787748	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.NewOrderBook_InitializedWithLayers_ContainsSameInstanceLayersAsInitialized	439
8935027621324685126	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.NewOrderBook_InitializedFromOrderBook_ClonesAllLayers	440
1819288246643318899	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedOrderBook_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull	441
9082917390085156165	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedOrderBook_SetAllLayers_ReplacesLayersWithNewSet	442
2677094157800687234	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedOrderBook_Capacity_ShowMaxPossibleNumberOfLayersNotNull	443
-5118235684681422184	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedOrderBook_CapacityLargerThanMaxBookDepth_ThrowsException	444
-7672359881715126002	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedOrderBook_Count_UpdatesWhenPricesChanged	445
-3274095225854377694	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedOrderBookClearHasUpdates_HasUpdates_ChangeItemAtATimeReportsUpdates	446
4773013725081228387	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.StaticDefault_LayerConverter_IsPQLayerConverter	447
8295892158328255943	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedOrderBook_Reset_ResetsAllLayers	448
5946293114760361894	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedQuoteWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllOrderBookFields	449
-2857515491886374742	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedQuoteWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllOrderBookFields	450
-9016000243843539149	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedOrderBookWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	451
7991943861722550330	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.PopulatedOrderBook_GetDeltaUpdatesUpdateFieldNewOrderBook_CopiesAllFieldsToNewOrderBook	452
-5032889823836522363	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyOrderBook_CopyFromToEmptyQuote_OrderBooksEqualEachOther	453
-1069215540778290849	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyPopulatedOrderBook_CopyFromLessLayers_ReplicatesMissingValues	454
7933044701474115079	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGap	455
3383483138513852096	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap	456
-5021294807930804702	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyOrderBook_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	457
3232398040969668304	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.NonPQOrderBook_CopyFromToEmptyOrderBook_OrderBooksEquivalentToEachOther	458
5701699701766514880	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.ForEachOrderBookType_EmptyOrderBookCopyFromAnotherOrderBookType_UpgradesToEverythingOrderBookItems	459
1224547687312934289	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.EmptyLayer_Mutate_UpdatesFields	834
1079777846125395147	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.ForEachOrderBookType_PopulatedCopyFromAnotherOrderBookType_UpgradesToEverythingOrderBookItems	460
-4757185636571062174	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal	461
631308306954226985	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	462
6592849061380933631	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyPopulatedOrderBookSameObj_Equals_ReturnsTrue	463
100475979119385272	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyPopulatedOrderBook_GetHashCode_ReturnNumberNoException	464
4488047058959874712	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyPopulatedQuote_ToString_ReturnsNameAndValues	465
6235711471212629325	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQOrderBookTests.FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries	466
3450623954380032235	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	467
-7805929135540215937	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	468
-2912815792102701509	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	469
-3149577814460868489	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.EmptyPvl_PriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	470
2419264815177948334	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.EmptyPvl_VolumeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	471
2892885172302753480	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.PopulatedPvl_HasUpdates_ClearedAndSetAffectsAllTrackedFields	472
2609711005454547867	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected	473
-517343203919092306	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	474
-9124560809556394670	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields	475
-8330373889145914487	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields	476
8747320628437539152	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	477
-6945858700441151447	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewPvl_CopiesAllFieldsToNewPvl	478
-6081354969982681999	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther	479
4035584893480932499	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	480
-7056374640302649604	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.NonPQPopulatedPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther	481
-4791221260792473098	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal	482
-5333581857906755938	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	483
657025011518556863	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.FullyPopulatedPvlSameObj_Equals_ReturnsTrue	484
-8970379998755300367	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	485
-7040402825184554156	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQPriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	486
-1460346436665267622	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	487
-9022769976017791560	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	488
8308133133012897805	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	489
-5498163394167153218	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.PopulatedPvl_HasUpdatesSetFalse_LookupAndLayerHaveNoUpdates	490
5204670132024648480	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.EmptyPvl_SourceNameChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	491
-5006680664044913737	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.EmptyPvl_LayerExecutableChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	492
8218793679501148847	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected	493
-3184534676668196556	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	494
-1507875266406929359	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields	495
6412966820676540751	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields	496
-4155611304333476112	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	835
5746705507760678982	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	497
-1400974169623107081	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl	498
6386329771030373937	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromToEmptyQuote_PvlsEqualEachOther	499
1929673210871152925	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	500
302047598451694000	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.EmptyPvl_EnsureRelatedItemsAreConfigured_SetsSourceNameIdLookupWhenNullOrSameAsInfo	501
2062383995271269786	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.EmptyPvlMissingLookup_EnsureRelatedItemsAreConfigured_SetsTraderNameIdLookupWhenNullOrSameAsInfo	502
-3325137484275582249	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.EmptyPvl_EnsureRelatedItemsAreConfigured_SharesTraderNameIdLookupBetweenLayers	503
3720236562660988131	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal	504
5455646959622234629	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	505
-8946073386512960103	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.FullyPopulatedPvlSameObj_Equals_ReturnsTrue	506
-6933499149927628326	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	507
3619311731473916220	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourcePriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	508
-3540429236196209402	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	509
1680103686424152264	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	510
5550703416682629513	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	511
-1261190560869627214	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.EmptyPvl_LayerSourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	512
-7382620743575113589	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected	513
-6481782723578544090	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	514
-6271626231804380759	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields	515
-2362168989856320897	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields	516
-3747520104157667041	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	517
-6274934036837929527	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl	518
9063175315430654317	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvl_CopyFromToEmptyQuote_PvlsEqualEachOther	519
3600368723188857852	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	520
-3275278053552906791	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal	521
-4089443421389216307	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	522
2209504073079976925	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvlSameObj_Equals_ReturnsTrue	523
6742207834329338799	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	524
-1931136746910890635	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	525
-8923877033546041150	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	526
4361595946815463385	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	527
-7126657288987151105	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	528
-8474275296072444440	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedSrcQuoteRefPvl_NewFromCloneInstance_PvlsEquivalentEachOther	529
-829046258088547010	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedSrcNamePvl_NewFromCloneInstance_PvlsEquivalentEachOther	530
-3429772387199373398	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedValueDatePvl_NewFromCloneInstance_PvlsEquivalentEachOther	531
4287622489691514106	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedTraderPvl_NewFromCloneInstance_PvlsEquivalentEachOther	532
4305927349006483485	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyPvl_ValueDateChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	533
-5891975601941594263	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyPvl_LayerSourceQuoteRefChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	534
3552664310779555538	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyPvl_SourceNameChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	535
77098920388313739	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyPvl_LayerExecutableChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	536
1008163528827344863	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedPvl_HasUpdatesSetFalse_LookupAndLayerHaveNoUpdates	537
2378735368864199773	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected	538
-411873028061658547	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	539
-4181691547645504898	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields	540
375420428497839569	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields	541
-4814854482884217950	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	542
-5052095623130965223	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl	543
8665316472139466444	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromNonPQSrcQtRefTrdrVlDtToEmptyQuote_PvlsEqualEachOther	544
-2352759059289503719	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_NewAndCopyFromSrcQtRefToEmptyQuote_PvlsEqualEachOther	545
-9185417756159682726	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromSrcPvlToEmptyQuote_PvlsEqualEachOther	546
-2537876146835428001	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromValueDatePvlToEmptyQuote_PvlsEqualEachOther	547
1988307758015769345	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromTraderPvlToEmptyQuote_PvlsEqualEachOther	548
7375152660932761054	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	549
-2204572513767827810	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyPvl_EnsureRelatedItemsAreConfigured_SetsSourceNameIdLookupWhenNullOrSameAsInfo	550
-4083001903083056848	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyPvlMissingLookup_EnsureRelatedItemsAreConfigured_SetsTraderNameIdLookupWhenNullOrSameAsInfo	551
-6417616407480563538	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyPvl_EnsureRelatedItemsAreConfigured_SharesTraderNameIdLookupBetweenLayers	552
-1090872573128234889	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal	553
-1870455361179779121	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	554
-3911209253402778809	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvlSameObj_Equals_ReturnsTrue	555
1819032009935960307	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	556
-7772786161986220840	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQSourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	557
2887177096307434411	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.NewTli_SetsPriceAndVolume_PropertiesInitializedAsExpected	558
-8330862128140684673	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.NewTli_NewFromCloneInstance_PropertiesInitializedAsExpected	559
-3752822565344380246	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.NewTli_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	560
-2642196237391216400	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.EmptyTli_TradeNameChanged_ExpectedPropertiesUpdated	561
-7514727290563466446	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.EmptyTli_TradeVolumeChanged_ExpectedPropertiesUpdated	562
-8803297981895917015	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.EmptyAndPopulatedTli_IsEmpty_ReturnsAsExpected	563
1377339183922792491	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.PopulatedTli_Reset_ReturnsReturnsLayerToEmpty	564
4139768908245332268	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.FullyPopulatedTli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther	565
-7258685998976334159	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.FullyPopulatedTli_Clone_ClonedInstanceEqualsOriginal	566
8476909923968604590	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.FullyPopulatedTliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	567
-6157168172706026864	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.FullyPopulatedTliSameObj_Equals_ReturnsTrue	568
-3602472103318952577	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.FullyPopulatedTli_GetHashCode_ReturnNumberNoException	569
-6377809419111902802	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderLayerInfoTests.FullyPopulatedTli_ToString_ReturnsNameAndValues	570
5629305678908643644	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	571
1466672490978449442	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	572
3936942997757393197	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	573
-4021020179368974406	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.EmptyPvl_IndexerGetSets_AddNewLayersIfIndexedViaVariousInterfaces	574
-6841055803091445449	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.EmptyPvl_IndexerGetSetOutOfIndex_ThrowsArgumentOutOfRangeException	575
3254722748319759749	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvl_HasUpdatesSetFalse_LookupAndTraderLayersHaveNoUpdates	576
8770879149278552638	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvl_SetTradersCountOnly_ExpectToBeCopied	577
-547175503758867039	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.EmptyPvl_LayerTraderNameChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	578
8267360228842387378	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.EmptyPvl_LayerTraderVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	579
-8840173634241471262	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvl_LayerTraderCountChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	580
-5353300839657544953	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvl_RemoveAt_ClearsOrReducesCount	581
-674673763740317037	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvl_AddAndReset_AddsToLastNonEmptySlot	582
-316447274411310824	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.EmptyPvl_SetTradersCountOnly_UpdatesCount	583
-1591481693817104529	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvl_IsTraderCountOnly_DeterminesIfTraderCountOnlyWhenSet	584
2223566409522421057	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected	585
-1899365803510401341	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	586
-1203559981284246877	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields	587
5783387881951179883	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields	588
8035979234755379503	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	589
158014887412681138	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl	590
108268604432865115	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.FullyPopulatedPvl_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther	591
4027233132685793832	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	592
-8836624144465935612	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.LayerWithManyTraderDetails_CopyFromSmallerTraderPvl_ClearsDownExtraLayers	593
224841885053618169	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.LayerWithManyTraderDetails_CopyFromNonPQSmallerTraderPvl_ClearsDownExtraLayers	594
-473344733180064987	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.SomeTraderNameVolumeUpdates_CopyFrom_OnlyChangesUpdated	595
7303097711185187609	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.SomeTraderVolumeUpdates_CopyFrom_OnlyChangesUpdated	596
6410203933308010749	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.EmptyPvl_EnsureRelatedItemsAreConfigured_SetsTraderNameIdLookupWhenNullOrSameAsInfo	597
7144604583276889671	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.EmptyPvlMissingLookup_EnsureRelatedItemsAreConfigured_SetsTraderNameIdLookupWhenNullOrSameAsInfo	598
-923832771737137593	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.EmptyPvl_EnsureRelatedItemsAreConfigured_SharesTraderNameIdLookupBetweenLayers	599
472257259985170296	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal	600
-7183873401417815368	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	601
5639561393726250596	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.FullyPopulatedPvlSameObj_Equals_ReturnsTrue	602
-748429510391056660	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	603
-1471821691389305120	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	604
-1244110861695361205	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQTraderPriceVolumeLayerTests.FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries	605
1849356259714524830	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	606
5372648116563306155	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	607
3788322937277224048	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.NewPvl_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	608
1741724177889364421	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.EmptyPvl_ValueDateChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	609
-1544374647241629962	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected	610
2270205132328204056	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	611
4570899514858431606	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.PopulatedPvlWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields	612
-1151592955510978112	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields	613
3641081747835851970	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.PopulatedPvlWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	614
3618095136331371696	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.PopulatedPvl_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewQuote_CopiesAllFieldsToNewPvl	615
-6147070407927747851	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromToEmptyQuote_PvlsEqualEachOther	616
-8482464526912559302	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	617
1454769167281619589	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal	618
-7160657270513693954	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	619
-5707501221620982254	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.FullyPopulatedPvlSameObj_Equals_ReturnsTrue	620
-3393924028984365802	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	621
218113030405659023	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.PQValueDatePriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	622
-7326307373869845509	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.VariosLayerFlags_Select_ReturnsPriceVolumeLayerFactory	623
8994507315322889238	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.VariosLayerFlags_Select_ReturnsSourcePriceVolumeLayerFactory	624
7113924106942583828	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.VariosLayerFlags_Select_ReturnsSourceQuoteRefPriceVolumeLayerFactory	625
-1178101138277602910	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.VariosLayerFlags_Select_ReturnValueDatePriceVolumeLayerFactory	626
6683456173721167212	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.VariosLayerFlags_Select_ReturnTraderPriceVolumeLayerFactory	627
-5187881189794042804	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.VariosLayerFlags_Select_ReturnsSrcQtRefTrdrVlDtPvlFactory	628
-5866543025278487313	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.NonPQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ConvertsToPQPriceVolumeLayerType	629
-1733326771329823463	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector.PQOrderBookLayerFactorySelectorTests.PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ClonesPQPriceVolumeLayerType	630
5405163203147116641	FortitudeTests: FortitudeTests.ComponentTests.Markets.Trading.TradingClientServerTests.StartedTradingServer_ClientJoinsSendsOrder_ServerSendsConfirmation	1364
8702385181061615546	FortitudeTests: FortitudeTests.ComponentTests.Markets.Pricing.PricingClientServerPubSubscribeTests.Level2QuoteFullDepth_ConnectsViaSnapshotUpdateAndResets_SyncsAndPublishesAllFields	1365
8845825461403835908	FortitudeTests: FortitudeTests.ComponentTests.Markets.Pricing.PricingClientServerPubSubscribeTests.Lvl3TraderLayerQuoteFullDepthLastTraderTrade_SyncViaUpdateAndResets_PublishesAllFieldsAndResets	1366
3592681968420981920	FortitudeTests: FortitudeTests.ComponentTests.IO.Transports.Sockets.Conversations.TCPRequestResponderConnectionTests.ClientSendMessageDecodesCorrectlyOnServer	1367
3740543862357774599	FortitudeTests: FortitudeTests.ComponentTests.IO.Transports.Sockets.Conversations.TCPRequestResponderConnectionTests.ClientSendMessageServerRespondsDecodesCorrectlyOnClient	1368
6795260181976179461	FortitudeTests: FortitudeTests.ComponentTests.IO.Transports.Sockets.Conversations.UDPPubSubConnectionTests.ClientSendMessageDecodesCorrectlyOnServer	1369
4836779721170942861	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected	649
2330984446180266726	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected	650
-4941497842926012142	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.NewLt_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	651
3850299087064257896	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.EmptyLt_TradeVolumeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	652
1379885231265209318	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.EmptyLt_WasGivenChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	653
4730900279908313531	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.EmptyLt_WasPaidChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	654
-6692942897719607452	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.PopulatedLt_HasUpdates_ClearedAndSetAffectsAllTrackedFields	655
6596385218658280824	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.PopulatedLt_Reset_ReturnsReturnsLayerToEmpty	656
7742280597781519260	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected	657
-4939379533109856050	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.PopulatedLtWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields	658
-559271542322089807	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields	659
-4988170037811856119	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	660
-1642963215527835214	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.PopulatedLt_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewLt_CopiesAllFieldsToNewLt	661
160470546533425842	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther	662
-4597536811667411047	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.FullyPopulatedLt_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	663
-5225835116227179315	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.FullyPopulatedLt_Clone_ClonedInstanceEqualsOriginal	664
2012674202134181162	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	665
9219722906344172547	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.FullyPopulatedLtSameObjOrClones_Equals_ReturnsTrue	666
3789344299255261546	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	667
-1110379169338656866	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastPaidGivenTradeTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	668
-3259485908059675943	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected	669
7433331124027422644	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected	670
7999208031922245120	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.NewLt_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	671
5805668549733399878	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.EmptyLt_TraderNameChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	672
3314571711313360320	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.PopulatedLt_HasUpdates_ClearedAndSetAffectsAllTrackedFields	673
-3548259621211206315	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.PopulatedLt_Reset_ReturnsReturnsLayerToEmpty	674
4746829911096675271	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected	675
-7623144499908919361	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.PopulatedLtWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields	676
-6723657192789197229	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields	677
-6335971593227921026	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	678
-5775568776361052001	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.PopulatedLt_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewLt_CopiesAllFieldsToNewLt	679
-8070008019278497384	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther	680
5679072309791352125	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.FullyPopulatedLt_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	681
8876830551412023741	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.FromInterfacePopulatedLastTrade_Cloned_ReturnsNewIdenticalCopy	682
-3460807262607593561	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	683
3293389237072111462	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.FullyPopulatedLtSameObjOrClones_Equals_ReturnsTrue	684
-6436209041997647296	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	685
6548174634444797682	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTraderPaidGivenTradeTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	686
-8522120391317385162	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected	687
777704923058060297	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected	688
-900840466130694104	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.NewLt_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies	689
4832504558073963968	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.EmptyLt_TradePriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	690
-8381922247839435305	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.EmptyLt_TradeTimeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	691
-704062481181072710	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.PopulatedLt_HasUpdates_ClearedAndSetAffectsAllTrackedFields	692
2020796491414859841	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected	693
-2290156110695935850	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.PopulatedLt_Reset_ReturnsReturnsLayerToEmpty	694
2302167374604391005	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.PopulatedLtWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields	695
-5674698766906137194	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields	696
-9183290367865991264	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	697
7498069002045135382	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.PopulatedLt_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewLt_CopiesAllFieldsToNewLt	698
8894193892046569413	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther	699
-7704236595691425209	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.FullyPopulatedLt_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	700
7579344795684748176	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.FullyPopulatedLt_Clone_ClonedInstanceEqualsOriginal	701
6996668969732783144	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	702
-8009200561564919891	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.FullyPopulatedLtSameObjOrClones_Equals_ReturnsTrue	703
5603297846530487325	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	704
3274810119064705259	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQLastTradeTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	705
3146147149573717354	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.NewRecentlyTraded_InitializedWithEntries_ContainsSameInstanceEntryAsInitialized	706
-2228400562854313153	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.NewRecentlyTraded_InitializedFromRecentlyTraded_ClonesAllEntries	707
-8781065821573446522	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTraded_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull	708
-6650893164806474862	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTraded_Capacity_ShowMaxPossibleNumberOfEntriesNotNull	709
-3532027586826254054	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTraded_Count_UpdatesWhenPricesChanged	710
-7076876310571766028	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTradedClearHasUpdates_HasUpdates_ChangeItemAtATimeReportsUpdates	711
-7871678614102366785	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.StaticDefault_EntryConverter_IsPQLastTradeEntySelector	712
-947369421393944280	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTraded_Reset_ResetsAllEntries	713
-7249504719250979845	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTraded_Add_AppendsNewLastTradeToEndOfExisting	714
5163656879946869082	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTradedWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllRecentlyTradedFields	715
-6689405592075286178	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.NoUpdatesPopulatedRecentlyTraded_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllRecentlyTradedFields	716
2041071870319067295	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTradedWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates	717
7315194354421844525	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTraded_GetDeltaUpdatesUpdateUpdateFields_CopiesAllFieldsToNewRecentlyTraded	718
-1981447961786148634	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedRecentlyTraded_CopyFromToEmptyRecentlyTraded_RecentlyTradedEqualEachOther	719
-7761558375622485442	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedRecentlyTraded_CopyFromSubTypes_SubTypeSaysIsEquivalent	720
2847239473139498991	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedRecentlyTraded_CopyFromLessLayers_ReplicatesMissingValues	721
8511227886358174355	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGapAsEmpty	722
8663491027335178694	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap	723
-7082171019313824030	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedRecentlyTraded_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	724
-6964947218939580574	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.NonPQRecentlyTraded_CopyFromToEmptyRecentlyTraded_RecentlyTradedAreEqual	725
1566535363509039560	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTraded_EmptyCopyFromOtherRecentlyTradedType_UpgradesToEverythingRecentlyTradedItems	726
-5864548989101485226	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.PopulatedRecentlyTraded_CopyFromRecentlyTraded_UpgradesToEverythingRecentlyTradedItems	727
-8884838108438765824	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulateRecentlyTraded_Clone_ClonedInstanceEqualsOriginal	728
-6892007837533608991	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.ClonedPopulatedRecentlyTraded_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	729
6640252121598649676	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedRecentlyTradedSameObj_Equals_ReturnsTrue	730
6974283897199924901	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedRecentlyTraded_GetHashCode_ReturnNumberNoException	731
-2413253437294506737	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedQuote_ToString_ReturnsNameAndValues	732
-5745046589739276255	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.PQRecentlyTradedTests.FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries	733
-2816095407172438235	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastPaidGivenTradeFactoryTests.NewPQLastPaidGivenTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected	734
-1526317184543969641	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastPaidGivenTradeFactoryTests.InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields	735
-4236279152836641538	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.VariosLastTradeFlags_Select_ReturnsSimpleLastTradeEntryFactory	736
5883450169777154787	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.VariosLastTradeFlags_Select_ReturnsSimpleLastPaidGivenTradeEntryFactory	737
-8885949884843028693	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.VariosLastTradeFlags_Select_ReturnsSimpleLastTraderPaidGivenTradeEntryFactory	738
-4275480352121319611	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.NonPQLastTradeTypes_ConvertIfNonPQLastTrade_ConvertsToPQLastTradeType	739
4464399667694645336	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.PQLastTradeTypes_ConvertIfNonPQLastTrade_ClonesPQLastTradeType	740
-8230243735332585566	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.PQLastTradeTypes_ConvertIfNonPQLastTrade_ReturnsSamePQLastTradeType	741
3843506714796885343	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.NonPQLastTradeTypes_TypeCanWholeyContain_ReturnsAsExpected	742
-6438327568176191801	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.PQLastTradeTypes_TypeCanWholeyContain_ReturnsAsExpected	743
-936893693115527437	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.NonPQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToTraderLastPaidEntryIfCantContain	744
-8722452410758653635	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.PQLastTradeEntries_SelectLastTradeEntry_UpgradesLayerToTraderLastPaidEntryIfCantContain	745
9175510766633525267	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeEntrySelectorTests.NullLastTradeEntries_SelectLastTradeEntry_HandlesEmptyValues	746
407113350092855760	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeFactoryTests.NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected	747
190569239989277010	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTradeFactoryTests.InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields	748
-1675329261346332849	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTraderPaidGivenTradeFactoryTests.NewPQLastPaidGivenTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected	749
-6997337568353676874	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector.PQLastTraderPaidGivenTradeFactoryTests.InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields	750
5400288476079911027	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.PopulatedPQNameIdGenerator_New_CopiesValues	751
-5094202754092917760	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.NewlyPopulatedPQNameIdLookup_HasUpdates_ExpectNoStringUpdatesWhenSetFalse	752
-6281351739100713367	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.NoNewlyPopulatedLookups_AddNewEntry_MarksNewEntryForStringUpdate	753
-8356487029510250283	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.PerDictionaryIdSubDictId_GetStringUpdates_PlacesDictionaryAndSubDictIdsInCorrectUpdateField	754
-2768266330783356096	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.NoNewlyPopulatedLookups_GetStringUpdatesAsFullSnapshot_ReturnsAllEntries	755
2991194027017732164	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.NoNewlyPopulatedLookups_UpdateFieldStringDifferentSubKey_IgnoresAllUpdates	756
6817250711474100479	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.NoNewlyPopulatedLookups_UpdateFieldStringDifferentDictId_IgnoresAllUpdates	757
-8609459946517706479	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.NonUpdateInsertAttempt_UpdateFieldString_IgnoresRequest	758
1117597016868852500	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.EmptyPQLookupGenerator_CopyFromSameInstance_NoChange	759
5480063571633879688	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.EmptyPQLookupGenerator_CopyFromPopulatedPQLookupGeneratorDefault_AreEqual	760
-786012976648189977	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.UpdatedDicttionary_CopyFromOnlyUpdated_OnlyChangesCopiedAcross	761
-7867237249420254429	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.UpdatedDicttionary_CopyFromNonUpdatedAsWell_OnlyChangesCopiedAcross	762
78506733819387106	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.PopulatedPQLookupGenerator_CopyFromNoAppend_ClearsPreviousValuesBeforeCopy	763
7343414712412368710	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.UpdatedPQLookupGenerator_CopyFrom_CopyKeepsUpdatedTracking	764
-3616245965538611598	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.NonPQLookupGenerator_EmptyCopyFrom_CopiesEverythingNothingMarkedUpdated	765
9062243209505088646	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.NonPQLookupGenerator_PopulatedCopyFrom_NoAppendCopiesRemainingIsOnlyWhatWasPassedIn	766
-5589140067204984410	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	767
-4620830751155899334	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.FromBaseTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	768
-4685899739859365940	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.FromTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	769
5988353012999221631	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.DifferingPqDictsNumAndSubNumber_AreEquivalent_ReturnsExpected	770
1625551037688455344	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DictionaryCompression.PQNameIdLookupGeneratorTests.FullyPopulatedPQNameLookupGenerator_ToString_ReturnsNameAndValues	771
4612079968168220183	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DeltaUpdates.PQFieldConvertersTests.DateNearEndOfHour_WhenConvertedToNumbersAndConvertedBackToDate_MaintainsNanosecondsNearEndOfHour	772
-8903470396850240656	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DeltaUpdates.PQFieldConvertersTests.DateNearEndOfHour_WhenConvertedToNumbersAndNumbersSplitAndReconstituted_NumberIsStillTheSame	773
7471397317073184649	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DeltaUpdates.PQFieldFlagsTests.AskFieldUpdate_IsAsk_ReturnsIsAskTrueAndIsBidFalse	774
-9073260828057306664	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DeltaUpdates.PQFieldFlagsTests.BidFieldUpdate_IsBid_ReturnsIsAskFalseAndIsBidTrue	775
-6004689372673379646	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.DeltaUpdates.PQScalingTests.RangeOfValues_AutoScaling_GivesExpectedValues	776
-638364817117994440	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_StartTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	777
-2322817597090801987	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_EndTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	778
-6912443921216102974	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_StartBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	779
-5206855240369371018	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_StartAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	780
3301255963549947634	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_HighestBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	781
4215354479734997830	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_HighestAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	782
6581433749055524307	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_LowestBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	783
5546863820523493479	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_LowestAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	784
255751341649350614	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_EndBidPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	785
-6627260910814133887	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_TickCountChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	786
1451008053836700258	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_EndAskPriceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected	787
5912273332061696898	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_PeriodVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected	788
4914174051134168774	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptySummary_DifferingStartEndTimesCalcTimeFrame_ReturnsExpectedTimeFrame	789
-1505864365404196627	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.PopulatedPeriodSummaryWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllLevel0Fields	790
-727451794362875038	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.PopulatedPeriodSummaryWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllLevel0Fields	791
2894744210331295187	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.PopulatedPeriodSummaryWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields	792
1772480079821854380	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.PopulatedPeriodSummary_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote	793
6535182933820282663	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.FullyPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_QuotesEqualEachOther	794
5501484454222036205	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.FullyPopulatedPeriodSummary_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData	795
665505556097720381	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.NonPQPopulatedPeriodSummary_CopyFromToEmptyQuote_QuotesEquivalentToEachOther	796
5086047834088109126	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.FullyInitializedQuote_Clone_CopiesQuoteExactly	797
-1140359889630538607	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	798
7731012049117049076	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.FullyPopulatedQuoteSameObj_Equals_ReturnsTrue	799
4038449796716148440	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Conflation.PQPeriodSummaryTests.EmptyQuote_GetHashCode_ReturnNumberNoException	800
-2664297099276952766	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FromSourceTickerQuoteInfo_New_InitializesOrderBookWithExpectedLayerTypes	801
-2482500879906466549	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.PQLayers_New_ConvertsToExpectedEquivalent	802
2425597280237009217	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.PQOrderBook_InitializedFromOrderBook_ConvertsLayers	803
-586652006490389178	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.NewOrderBook_InitializedWithLayers_ContainsSameInstanceLayersAsInitialized	804
6580682854756946552	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.NewOrderBook_InitializedFromOrderBook_ClonesAllLayers	805
686799993030402996	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.PopulatedOrderBook_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull	806
4874361422224701319	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.PopulatedOrderBook_Capacity_ShowMaxPossibleNumberOfLayersNotNull	807
8502745391283177618	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.PopulatedOrderBook_CapacityLargerThanMaxBookDepth_ThrowsException	808
8614401276580995037	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.PopulatedOrderBook_Count_UpdatesWhenPricesChanged	809
-1327063142942510431	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.StaticDefault_LayerConverter_IsOrderBookLayerFactorySelector	810
1932581422101276016	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.PopulatedOrderBook_Reset_ResetsAllLayers	811
-2291857299264558162	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedOrderBook_CopyFromToEmptyQuote_OrderBooksEqualEachOther	812
-4080222699972974441	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedOrderBook_CopyFromSubTypes_SubTypeSaysIsEquivalent	813
7720997577829840202	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedOrderBook_CopyFromLessLayers_ReplicatesMissingValues	814
3408792733589247099	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGap	815
8691023200992283088	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap	816
-242433313724515810	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal	817
2673070882760588525	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedQuoteCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	818
3049192474232067376	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedOrderBook_GetHashCode_ReturnNumberNoException	819
-2575370679915686837	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedQuote_ToString_ReturnsNameAndValues	820
-3076343116555089877	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.OrderBookTests.FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries	821
5287458118285883886	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	822
694918346179025770	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	823
-6342531851170206187	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.EmptyLayer_Mutate_UpdatesFields	824
-1692105356781825231	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	825
-992377258616668112	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther	826
-6311014117888634806	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.PQPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther	827
4848000494819026557	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	828
1059691320582458907	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	829
-3032344655395207967	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	830
-2372598623342294744	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.PriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	831
5425160292901546755	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	832
76488856091185374	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	833
-5570697310300715893	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther	836
-4576905349066182435	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.PQPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther	837
7610556504345267071	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	838
-7470257043996736661	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	839
416687423359059407	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	840
7228942233047580204	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourcePriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	841
4546933048618210470	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	842
-3440651093663044021	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	843
-292277682007308740	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.EmptyLayer_Mutate_UpdatesFields	844
4324309647875998771	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	845
-1178792961506915486	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther	846
-1452112783999086304	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.PQPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther	847
4208383395991285259	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	848
2681240363186309918	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	849
5948516854699542069	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	850
-8282321649916488413	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefPriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	851
-7870819632915365856	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	852
7436420335013592145	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	853
-2047916398335965857	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedSrcQuoteRefPvl_NewFromCloneInstance_PvlsEquivalentEachOther	854
-9015728697059075019	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedSrcNamePvl_NewFromCloneInstance_PvlsEquivalentEachOther	855
728247740357176199	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedValueDatePvl_NewFromCloneInstance_PvlsEquivalentEachOther	856
-9165043103091401601	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedTraderPvl_NewFromCloneInstance_PvlsEquivalentEachOther	857
-3020579081032030055	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyLayer_Mutate_UpdatesFields	858
4413574436139789906	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected	859
8388470501585506122	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	860
-4875998285682959509	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromNonPQSrcQtRefTrdrVlDtToEmptyQuote_PvlsEqualEachOther	861
-8011489024666563166	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_NewAndCopyFromSrcQtRefToEmptyQuote_PvlsEqualEachOther	862
4308079095349473247	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromSrcPvlToEmptyQuote_PvlsEqualEachOther	863
1091469637333404721	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromValueDatePvlToEmptyQuote_PvlsEqualEachOther	864
-4473303655182815444	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromTraderPvlToEmptyQuote_PvlsEqualEachOther	865
-2275697846518862928	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal	866
-9184799163675668790	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	867
4057932499337166717	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	868
-6006606298310346554	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.SourceQuoteRefTraderValueDatePriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	869
-7255101302504907991	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderLayerInfoTests.NewTli_SetsPriceAndVolume_PropertiesInitializedAsExpected	870
4712272820753174645	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderLayerInfoTests.NewTli_NewFromCloneInstance_PropertiesInitializedAsExpected	871
-7673554198257650506	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderLayerInfoTests.EmptyAndPopulatedTli_IsEmpty_ReturnsAsExpected	872
-7262478061143821468	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderLayerInfoTests.PopulatedTli_Reset_ReturnsReturnsLayerToEmpty	873
5858929025123377530	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderLayerInfoTests.FullyPopulatedTli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther	874
-8351617673807613926	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderLayerInfoTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	875
-6339998741219588628	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderLayerInfoTests.FullyPopulatedTliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	876
-1930517508288140553	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderLayerInfoTests.FullyPopulatedTli_GetHashCode_ReturnNumberNoException	877
-5004865016691376838	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderLayerInfoTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	878
-1229892140439693211	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	879
-7521650831978226364	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	880
4176625386367073818	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.EmptyPvl_IndexerGetSets_AddNewLayersIfIndexedViaVariousInterfaces	881
4794068930792434294	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.EmptyPvl_IndexerGetSetOutOfIndex_ThrowsArgumentOutOfRangeException	882
-4988149378914569423	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.PopulatedPvl_IsTraderCountOnly_TrueWhenSetTradersCountOnly	883
-6860337138445249149	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.PopulatedPvl_AddAndReset_AddsToLastNonEmptySlot	884
3964857468385941503	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.PopulatedPvl_RemoveAt_ClearsOrReducesCount	885
-4989319812405585052	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.EmptyPvl_SetTradersCountOnly_UpdatesCount	886
1742266396999405263	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.PopulatedPvl_IsTraderCountOnly_DeterminesIfTraderCountOnlyWhenSet	887
3886518409372390089	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected	888
-1135443937584918265	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	889
2006855055502907474	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther	890
-768227824610522298	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.PQPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther	891
4716354392741012469	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.LayerWithManyTraderDetails_CopyFromSmallerTraderPvl_ClearsDownExtraLayers	892
-3666150636673913382	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	893
5742032320271158230	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	894
8039513648741184249	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	895
-4757149717594373648	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	896
912800248653254451	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.TraderPriceVolumeLayerTests.FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries	897
2171890429284314195	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected	898
6104628488879558047	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected	899
-3467873180011179379	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.EmptyLayer_Mutate_UpdatesFields	900
-5525605133353891528	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty	901
-6346860121539058337	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther	902
4814594667029517543	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.PQPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther	903
544509640253020970	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	904
265829263482519944	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	905
44681976019897786	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	906
-5744614909404880627	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.ValueDatePriceVolumeLayerTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	907
5835430415156872082	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.LayerFlagsSelectorTests.VariosLayerFlags_Select_CallsSimplePriceVolumeSelector	908
3465093300269864384	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.LayerFlagsSelectorTests.VariosLayerFlags_Select_CallsSourcePriceVolumeSelector	909
-3078742353904403650	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.LayerFlagsSelectorTests.VariosLayerFlags_Select_CallsSourceQuoteRefPriceVolumeSelector	910
882326949164834635	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.LayerFlagsSelectorTests.VariosLayerFlags_Select_CallsValueDatePriceVolumeSelector	911
7664680734285423530	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.LayerFlagsSelectorTests.VariosLayerFlags_Select_CallsTraderPriceVolumeSelector	912
883003652861180376	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.LayerFlagsSelectorTests.VariosLayerFlags_Select_CallsSrcQtRefTrdrVlDtSelector	913
-7293576720655050036	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.OrderBookLayerFactorySelectorTests.VariosLayerFlags_FindForLayerFlags_ReturnsPriceVolumeLayer	914
7923350681241079024	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.OrderBookLayerFactorySelectorTests.VariosLayerFlags_FindForLayerFlags_ReturnsSourcePriceVolumeLayer	915
-2056343294866171643	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.OrderBookLayerFactorySelectorTests.VariosLayerFlags_FindForLayerFlags_ReturnsSourceQuoteRefPriceVolumeLayer	916
9081816143670670034	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.OrderBookLayerFactorySelectorTests.VariosLayerFlags_FindForLayerFlags_ReturnValueDatePriceVolumeLayer	917
-967846679142506426	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.OrderBookLayerFactorySelectorTests.VariosLayerFlags_FindForLayerFlags_ReturnTraderPriceVolumeLayer	918
-7204819334085466013	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.OrderBookLayerFactorySelectorTests.VariosLayerFlags_FindForLayerFlags_ReturnsSrcQtRefTrdrVlDtPvl	919
5880049292494101918	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.OrderBookLayerFactorySelectorTests.PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ConvertsToNonPQPriceVolumeLayerType	920
8221084992272232112	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.OrderBookLayerFactorySelectorTests.NonPQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ClonesPriceVolumeLayerType	921
609604938207610433	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector.OrderBookLayerFactorySelectorTests.PriceVolumeLayerTypes_ConvertToExpectedImplementation_ReturnsSamePriceVolumeLayerType	922
-347706343151557748	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected	923
-4569641039552417056	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected	924
-4976273116129123078	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.EmptyEntry_Mutate_UpdatesFields	925
-2625437675942997275	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.PopulatedLt_Reset_ReturnsReturnsLayerToEmpty	926
7729616558499493031	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected	927
3836076711622384050	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther	928
-8805606788410929812	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.PQPopulatedLt_CopyFromToEmptyPvl_QuotesEquivalentToEachOther	929
63851718022001911	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.FromInterfacePopulatedLastTrade_Cloned_ReturnsNewIdenticalCopy	930
9205696324497013	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	931
8724533709734202211	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.FullyPopulatedLtSameObjOrClones_Equals_ReturnsTrue	932
256728256771641962	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	933
-1131743231451411598	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastPaidGivenTradeTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	934
-2449078386832694601	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected	935
6064964023703205388	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected	936
8966706877660743097	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.EmptyEntry_Mutate_UpdatesFields	937
-4025443318567146955	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.PopulatedLt_Reset_ReturnsReturnsLayerToEmpty	938
5485868942381942023	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected	939
5879881600104610197	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther	940
-2939408616828985634	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.PQPopulatedLt_CopyFromToEmptyPvl_QuotesEquivalentToEachOther	941
5406123222108191294	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.FromInterfacePopulatedLastTrade_Cloned_ReturnsNewIdenticalCopy	942
4343929996627703905	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	943
-7672027769739477145	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.FullyPopulatedLtSameObjOrClones_Equals_ReturnsTrue	944
7437103956357837389	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	945
7915545078928575986	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTraderPaidGivenTradeTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	946
-8545790135179170598	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected	947
701031601927376825	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected	948
-9112096851723170186	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected	950
3384374749149532045	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.PopulatedLt_Reset_ReturnsReturnsLayerToEmpty	951
-327349863373322152	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther	952
-6813084329999632373	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.PQPopulatedLt_CopyFromToEmptyPvl_QuotesEquivalentToEachOther	953
-7917844593539014293	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.FromInterfacePopulatedLastTrade_Cloned_ReturnsNewIdenticalCopy	954
-3560994649842946011	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	955
2367462802463363422	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.FullyPopulatedLtSameObjOrClones_Equals_ReturnsTrue	956
-3613740110692547712	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.FullyPopulatedPvl_GetHashCode_ReturnNumberNoException	957
6589356139030126645	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.LastTradeTests.FullyPopulatedPvl_ToString_ReturnsNameAndValues	958
3528086416049257714	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.NewRecentlyTraded_InitializedWithEntries_ContainsSameInstanceEntryAsInitialized	959
7071521955825074980	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.NewRecentlyTraded_InitializedFromRecentlyTraded_ClonesAllEntries	960
4670464998368903761	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.PopulatedRecentlyTraded_AccessIndexerVariousInterfaces_GetsAndSetsLayerRemovesLastEntryIfNull	961
8052128727758468466	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.PopulatedRecentlyTraded_Capacity_ShowMaxPossibleNumberOfEntriesNotNull	962
-4027489574189355398	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.PopulatedRecentlyTraded_Count_UpdatesWhenPricesChanged	963
-7772401465520405793	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.StaticDefault_EntryConverter_IsPQLastTradeEntySelector	964
-7864230203590923057	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.PopulatedRecentlyTraded_Reset_ResetsAllEntries	965
8910539476105716733	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.PopulatedRecentlyTraded_Add_AppendsNewLastTradeToEndOfExisting	966
-4928145995967954691	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FullyPopulatedRecentlyTraded_CopyFromToEmptyRecentlyTraded_RecentlyTradedEqualEachOther	967
-986376273584432818	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FullyPopulatedRecentlyTraded_CopyFromSubTypes_SubTypeSaysIsEquivalent	968
-4640904247616785874	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FullyPopulatedRecentlyTraded_CopyFromLessLayers_ReplicatesMissingValues	969
8190960998390422427	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FullyPopulatedOrderBook_CopyFromWithNull_ReplicatesGap	970
-4608901660714913337	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FullyPopulatedOrderBook_CopyFromAlreadyContainsNull_FillsGap	971
1275011559545855911	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.PQRecentlyTraded_CopyFromToEmptyRecentlyTraded_RecentlyTradedAreEqual	972
-1295201336329561053	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	973
9091829820384950553	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.ClonedPopulatedRecentlyTraded_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent	974
6540957371066614316	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FullyPopulatedRecentlyTradedSameObj_Equals_ReturnsTrue	975
-2019419135082391874	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FullyPopulatedRecentlyTraded_GetHashCode_ReturnNumberNoException	976
-2398904949545213388	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FullyPopulatedQuote_ToString_ReturnsNameAndValues	977
-9190790590285646942	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.RecentlyTradedTests.FullyPopulatedPvlVariousInterfaces_GetEnumerator_OnlyGetsNonEmptyEntries	978
7849459939427238065	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.EntrySelector.LastTradeEntryFlagsSelectorTests.VariosLayerFlags_Select_CallsSimpleLastTradeSelector	979
-8299700317371339995	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.EntrySelector.LastTradeEntryFlagsSelectorTests.VariosLayerFlags_Select_CallsLastPaidGivenTradeSelector	980
-377606298713362625	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.EntrySelector.LastTradeEntryFlagsSelectorTests.VariosLayerFlags_Select_CallsLastTraderPaidGivenTradeSelector	981
-2031725293327449026	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.EntrySelector.RecentlyTradedLastTradeEntrySelectorTests.VariosLastTradeFlags_FindForLastTradeFlags_ReturnsLastTrade	982
5983264011791497714	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.EntrySelector.RecentlyTradedLastTradeEntrySelectorTests.VariosLastTradeFlags_FindForLastTradeFlags_ReturnsLastPaidGivenTrade	983
-7690610360618111555	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.EntrySelector.RecentlyTradedLastTradeEntrySelectorTests.VariosLastTradeFlags_FindForLastTradeFlags_ReturnsLastTraderPaidGivenTrade	984
449937141918057277	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.EntrySelector.RecentlyTradedLastTradeEntrySelectorTests.PQPriceVolumeLayerTypes_ConvertToExpectedImplementation_ConvertsToNonPQPriceVolumeLayerType	985
6914732839229375633	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.EntrySelector.RecentlyTradedLastTradeEntrySelectorTests.NonPQLastTradeTypes_ConvertToExpectedImplementation_ClonesPQPriceVolumeLayerType	986
-8850891524784861637	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.LastTraded.EntrySelector.RecentlyTradedLastTradeEntrySelectorTests.PriceVolumeLayerTypes_ConvertToExpectedImplementation_ReturnsSamePriceVolumeLayerType	987
8533968584872633970	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryHelpersTests.UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly	988
681146999070498170	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.EmptyPeriodSummary_New_InitializesFieldsAsExpected	989
-3450014328039069239	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.PopulatedPeriodSummary_New_InitializesFieldsAsExpected	990
-8342820603299250897	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.UnknownTimeFrame_New_CalculatesCorrectTimeFrameForPeriodSummary	991
-1091438705083176208	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.PopulatedPeriodSummary_New_CopiesValues	992
5902272033214131380	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.UnknownTimeFrameVariousStartEndTimes_TimeFrame_CalculatedCorrectly	993
7908348201613442623	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.PopulatedQuote_Mutate_UpdatesFields	994
-5149731834386805752	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.FullyPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_PeriodSummariesEqualEachOther	995
-1512542050362921247	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.PQPopulatedPeriodSummary_CopyFromToEmptyPeriodSummary_PeriodSummariesEquivalentToEachOther	996
-2555596903551353834	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.PopulatedPeriodSummary_Clone_CreatesCopyOfEverythingExceptSrcTkrQtInfo	997
-8591353267454696957	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	998
4648438223477796593	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.OneDifferenceAtATime_AreEquivalent_ReturnsExpected	999
2245323675258877451	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.PopulatedQuote_GetHashCode_NotEqualToZero	1000
6959475857629491259	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Pricing.Conflation.PeriodSummaryTests.FullyPopulatedQuote_ToString_ReturnsNameAndValues	1001
-7125449764991703689	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SnapshotUpdatePricingServerConfigTests.InitializedSnapUpdateServerConfig_NewConfigSameIdThroughUpdateStream_UpdatesSnapUpdateValues	1002
-2806368011398906520	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SnapshotUpdatePricingServerConfigTests.When_Cloned_NewButEqualConfig	1003
5594314531942316630	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerClientAndPublicationConfigTests.DummySourceTickerQuoteInfo_New_PropertiesAreAsExpected	1004
-8988012340689947584	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerClientAndPublicationConfigTests.EmptySourceTickerQuoteInfo_New_DefaultAreAsExpected	1005
-4528637836425608369	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerClientAndPublicationConfigTests.When_Cloned_NewButEqualConfigCreated	1006
-1135189464947323845	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerClientAndPublicationConfigTests.NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame	1007
2885526710536598113	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerClientAndPublicationConfigTests.OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent	1008
1286238108062068368	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerClientAndPublicationConfigTests.PopulatedStcpc_GetHashCode_NotEqualTo0	1009
-6648944238338842481	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerClientAndPublicationConfigTests.FullyPopulatedSti_ToString_ReturnsNameAndValues	1010
6860362571554215216	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerPublicationConfigRepositoryTests.InitializingRepoFromEnumerable_Constructor_ReturnsPassedInValues	1011
321554501983926968	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerPublicationConfigTests.DummySourceTickerQuoteInfo_New_PropertiesAreAsExpected	1012
5367931265214041181	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerPublicationConfigTests.EmptySourceTickerQuoteInfo_New_DefaultAreAsExpected	1013
3538872270517270634	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerPublicationConfigTests.When_Cloned_NewButEqualConfigCreated	1014
7732139213757531631	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerPublicationConfigTests.NonExactUniqueSourceTickerId_AreEquivalent_EquivalentWhenSamePartsSame	1015
6714215559856081395	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerPublicationConfigTests.OneDifferenceAtATime_AreEquivalent_ReturnsFalseWhenDifferent	1016
-7641392127491465904	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerPublicationConfigTests.PopulatedStpc_GetHashCode_NotEqualTo0	1017
2276997983505728369	FortitudeTests: FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig.SourceTickerPublicationConfigTests.FullyPopulatedStpc_ToString_ReturnsNameAndValues	1018
-1007467999953333188	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.OneOrBothQuotesNull_Diff_ReturnsOneIsNullString	1019
2374965325163934168	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.TwoIdenticalQuotes_Diff_ReturnsEmptyString	1020
5616213889778005441	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.IdenticalExceptExchangeNameQuotes_Diff_ReturnsExchangeNameDifferent	1021
8455353873766678633	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.IdenticalExceptTickerQuotes_Diff_ReturnsTickerDifferent	1022
-8398102793415941379	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.IdenticalExceptExchangeTimeQuotes_Diff_ReturnsExchangeTimeDifferent	1023
-2780071311272975110	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.IdenticalExceptAdapterTimeQuotes_Diff_ReturnsAdapterTimeDifferent	1024
-8053000820966812681	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.IdenticalExceptReceivedTimeQuotes_Diff_ReturnsReceiveTimeDifferent	1025
-6664754518424090642	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.IdenticalExceptBidTopQuotes_Diff_ReturnsBidTopDifferent	1026
-6570008418861600440	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.IdenticalExceptAskTopQuotes_Diff_ReturnsAskTopDifferent	1027
6551778862476105365	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.IdenticalExceptBidBookQuotes_Diff_ReturnsBidBookDifferent	1028
6627834638240895512	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Pricing.QuoteExtensionMethodsTests.IdenticalExceptAskBookQuotes_Diff_ReturnsAskBookDifferent	1029
8315107481872866927	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig.MarketServerConfigRepositoryTests.NonEmptyRepo_FindConfig_RetirevesConfig	1030
5670771697453772560	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig.MarketServerConfigRepositoryTests.NonEmptyRepo_SubscribeToUpdateStream_RetirevesExistingConfig	1031
1544155196440893856	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig.MarketServerConfigRepositoryTests.EmptyRepo_AddOrUpdateThenDelete_DeleteEventsAreReceived	1032
-4770951617551334686	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig.MarketServerConfigRepositoryTests.EmptyRepo_AddOrUpdate_UpdateEventsAreReceived	1033
-372345773148852929	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig.MarketServerConfigRepositoryTests.EmptyRepo_AddOrUpdate_AddEventsAreReceived	1034
-5733967213181359467	FortitudeTests: FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig.MarketServerConfigTests.InitializedServerConfig_NewConfigSameIdPushedThroughUpdateStream_UpdatesAllValues	1035
-4176918439774775822	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.ConnectionConfigTests.ServerConnectionConfig_PublishUpdateStream_UpdatesExistingItem	1036
5691238000848924670	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SocketsConfigurationContextTests.NewSocketsConfigurationContext_Instance_CreatesNewInstance	1037
-3085202136643791384	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSelectorTests.NewSocketSelector_RegisterSocketSessionConnection_AddsSessionForListeningForIncomingRequests	1038
-6131068197541576738	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSelectorTests.TwoRegisteredSocketSelectors_UnregisterSocketSessionConnection_RemovesSocketSessionConnection	1039
-6862115297513520503	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSelectorTests.NoRegisteredSocketSessionConnection_SelectRecv_ThrowsException	1040
-464283600867059024	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSelectorTests.RegisteredSocketSessionConnection_SelectRecv_ReturnsWithSocketSelected	1041
7186828315059264889	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSelectorTests.FourRegisteredSocketSessionConnection_MultipleSelectRecv_RotatesOrderOfSelectedSocketsReturned	1042
6158124222323029640	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSelectorTests.RegisteredSocketSessionConnection_SelectRecvReturnsError_ExpectedException	1043
-8464602885554311459	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.NewSocketStreamSubr_StartMessaging_StartsDispatcher	1044
108747157671611731	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.StartedSocketStreamSubr_StopMessaging_StopsDispatcher	1045
8462740860802420442	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.RegisteredSocketStreamSubr_Unregister_RemovesSocketSessionConnectionFromDispatcher	1046
-2399654172080959678	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.NoDeserializersRegistered_RegisterDeserializer_AddsSerializerForMessageAndSyncProtectsCount	1047
-8910952457536193267	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.NoDeserializersRegistered_RegisterDeserializerWithNoCallBackMethod_ThrowsException	1048
-6906691816955658071	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.AlreadyRegisteredMsg_RegisterDeserializerWithDiffTypeCallback_ThrowsException	1049
-3635710217030240506	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.AlreadyRegisteredMsg_RegisterDeserializerTwice_ThrowsException	1050
5857327834671419286	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.AlreadyRegisteredMsg_UnregisterDeserializer_RemovesSerializer	1051
-5928123676542923322	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.NonRegisteredMsg_UnregisterDeserializer_ThrowsException	1052
-1222368133678978019	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.RegisteredDifferentCallbackTypeMsg_UnregisterDeserializer_ThrowsException	1053
5269657952108192472	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketStreamSubscriberTests.NonRegisteredCallbackTypeMsg_UnregisterDeserializer_ThrowsException	1054
2067208644914676018	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.NeverStarted_ConfigurationUpdate_CallsConnect	1055
-2638291134221394996	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.AlreadyConnected_ConfigurationUpdate_CallsDisconnectThenConnect	1056
2153771988474078008	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.NewSocketSubscriber_IsConnected_WhenSocketIsConnectedOrBound	1057
-960128883284789234	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.NewSocketStreamSubr_RegisterConnector_ReturnsSocketSessionConnection	1058
2460692043327998509	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.UnconnectedSocketSubscriber_BlockedUntilConnected_Connects	1059
-367814829649005664	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.Connected_OnCxErrorWith0ms_CallDisconnectWithNoDisconnectingEventThenImmediatelyConnect	1060
1237306865968645953	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.ConnectedSocketSubscriber_OnCxErrorWith1000ms_CallDisconnectThenSchedulesConnect	1061
-768449131088247831	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.Connected_OnCxErrorTwiceWith1000ms_CallDisconnectThenSchedulesConnectOnlyOnce	1062
-891949923407065854	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.NewSocketSubscriber_Connect_DispatchesConnectOnBackgroundThreadImmediately	1063
-7458424375290572939	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.ConnectingSubscriber_Connect2ndTime_IgnoresSecondConnectionRequest	1064
5898754603752214121	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.ConnectedSocketSubscriber_Connect_SchedulesConnectOnlyOnce	1065
6447188493202147399	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.UnconnectedSocketSubscriber_ConnectFails_SchedulesBackgroundReconnectAtReconnectInterval	1066
8522311862062046826	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.UnconnectedSocketSubscriber_ConnectFailsFirstConfig_FallbackConfigConnects	1067
3009239510960794656	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.SocketSubscriberTests.UnconnectedSocketSubscriber_ConnectThrowsExceptionFirstConfig_FallbackConfigConnects	1068
4233681671533271443	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.UdpSubscriberTests.NewUdpSubscriberWithValidMulticastAddress_CreateAndConnect_ReturnsSocketBoundToEndPoint	1069
-288608372800445033	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.UdpSubscriberTests.NoValidMulticastNic_CreateAndConnect_ThrowsException	1070
-1688916975541725591	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.UdpSubscriberTests.InValidMulticastNiAddress_New_DefaultsToHostnameResolution	1071
4230669388504180511	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Subscription.UdpSubscriberTests.NoMulticastNiAddress_New_DefaultsToHostnameResolution	1072
-1441439608458833047	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionConnectionBaseTests.NewSocketSession_New_IntializesPropertiesAsExpected	1073
-8693463573350018352	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionConnectionBaseTests.NewSocketSession_ToString_ContainsIdAndSessionDescription	1074
-6822581025649830660	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionConnectionBaseTests.TwoSocketSessions_EqualsGetHashcode_DifferentiateWhenDifferent	1075
5299779084982604411	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.SessionReceiverAsAcceptor_New_PropertiesSetupAsExpected	1076
7341252202662817654	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.SessionReceiverAsReceiver_New_PropertiesSetupAsExpected	1077
-3734011219728813011	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsAcceptorSocket_OnAccept_InvokesHandler	1078
-3111047399857813227	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithDataAndAvailableBuffer_ReceiveData_CallsDecoderToReadBuffer	1079
6356044858478115513	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithNoDataAndZeroBytesReadIsNotDisconnection_ReceiveData_ReturnsTrue	1080
7631961285996600547	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithNoDataAndZeroBytesReadIsDisconnection_ReceiveData_ReturnsFalse	1081
4078814598570768663	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithTooMuchData_ReceiveData_OnlyReportsOutburstEveryMinute	1082
-2671902171571531285	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithData_ReceiveData_LogsTraceIfDecoderProcessesNothing	1083
5121651198702756892	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketUnreadBuffer_ReceiveData_DoesNotResetBufferWhileUnreadRemains	1084
87388039916460421	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketRemainingBufferLessThan400_ReceiveData_ShiftsUnreadDataToBufferStart	1085
911759618204730973	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithIOCTLError_ReceiveData_ThrowsExceptionWithLasCallError	1086
4291486113038902560	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithRecvError_ReceiveData_ThrowsExceptionWithLastCallError	1087
-6881348954402300356	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithSocketTraceEnabledNearFullBuffer_ReceiveData_SetsTraceAndMessageSize	1088
-6709824203167960709	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithSocketTraceEnabledFullBuffer_ReceiveDataAMillionTimes_SetsTraceAndMessageSize	1089
-8128155084156210464	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithSocketTraceEnabledEmptyBuffer_ReceiveData_SetsTraceAndMessageSize	1090
-2453704373727120087	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithNearFullBuffer_FirstReceiveData_ThrowsExceptionToCloseReopenSocketEmpty	1091
8257310182211849555	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketWithNearFullBuffer_SecondReceiveData_DoesNotThrowsException	1092
2005531911630964251	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketSocketDumpEnable_ReceiveData_WriteBytesToLogger	1093
-8131967445052921272	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketMultipleWholeMessaagesLimitOnSessionMax_ReceiveData_RemovesExpectedNumMessages	1094
-52907678272727815	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketMultipleWholeMessaagesLimitOnDecoderMax_ReceiveData_RemovesExpectedNumMessages	1095
7116507851557313248	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketMultiWholeMsgsLmtRemainingBufferSz_ReceiveData_RemovesExpectedNumMessages	1096
-7215120721757903283	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionReceiverTests.AsReceiverSocketMultipleWholeMessaagesLimitSocketOutOfData_ReceiveData_RemovesAllMessages	1097
-8960805452828810794	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionSenderTests.NewSessionSender_Enqueue_QueuesItemForSendDuringSyncLock	1098
-7067785902223299339	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionSenderTests.OneQueuedMessage_SendData_SerializesMessageSendsToDirectOsNetworkingApi	1099
-1763050638754675372	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionSenderTests.NoQueuedMessages_SendData_ReturnsTrue	1100
5780901546479079195	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionSenderTests.MultipleQueuedMessages_SendData_CallsSendForAllMessages	1101
5524856594142199329	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionSenderTests.TwoMsgsCantSerialize2ndMsgFullBuffer_SendData_SendsFirstMsgToClearBuffThenSends2ndMsg	1102
3863217587438177010	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionSenderTests.OneMsgCantSerializeToBuffer_SendData_ThrowsExceptionMsgTooBig	1103
-6530690482946375377	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionSenderTests.OneMsgQueued_SendDataError_ThrowsExceptionWithError	1104
5437554715955851851	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.SessionConnection.SocketSessionSenderTests.OneMsgQueued_SendDataNotAllDataSent_ReturnsFalse	1105
3211889679154096856	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.ListeningSocketStreamPublisher_Unregister_InformsDispatcherToUnregister	1106
-2395467479759687223	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.ListeningSocketStreamPublisher_StartMessaging_StartsDispatcher	1107
-2573991658597989803	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.ListeningSocketStreamPublisher_StopMessaging_InformsDispatcherToUnregister	1108
-4377735459434946599	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.NewSocketStream_RegisterSerializer_FindsSerializerAddsItToLookup	1109
-72027690974534248	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.AlreadyRegisteredId_RegisterSerializer_ThrowsException	1110
207189933907901365	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.NewSocketStream_UnregisterSerializer_FindsSerializerAddsItToLookup	1111
2361921548870230688	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.UnregisteredMessageId_UnregisterSerializer_ThrowsException	1112
-3252907002280195029	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.RegisteredSerializer_EnqueSingleCx_AlertsCxWithMessageSerializerAndDispatcher	1113
4604669814823382222	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.UnregisteredSerializer_EnqueSingleCx_ThrowsException	1114
-6435126593051665603	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.SocketStreamPublisherTests.RegisteredSerializer_EnqueMultipleCx_AlertsEachCxWithMessageSerializerAndDispatcher	1115
7282032306123914706	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.NewSocketStream_RegisterAcceptor_ReturnsConfiguredSocketSessionConnection	1116
848326468934320527	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.UnconnectedSocketPublisher_Connect_SyncProtectsCreateSocketAndConnectionCallsOnConnect	1117
9190602656340832148	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.ConnectedSocketPublisher_Connect_DoesNothing	1118
-6473671706617561918	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.ConnectedSocketPublisher_Disconnect_SyncProtectsStopsDispatcherAlertsClientsCallsOnDisconnect	1119
-5726951638775068934	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.UnconnectedSocketPublisher_Disconnect_DoesNothing	1120
-9079328690913641210	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.ConnectedSocket_IsConnect_ReturnsTrue	1121
-9210625787600764149	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.BoundSocket_IsConnect_ReturnsTrue	1122
4513977049716890332	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.ConnectedRegisteredMessage_Send_EnquesMessageWithSerializer	1123
-9158713616993224031	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.ConnectedClient_OnCxError_UnregistersAndRemovesClient	1124
4374512322371477589	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.ConnectedServer_OnClientConnect_SyncProtectsCreatesClientSocketAddsToList	1125
6357542954624213120	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.ConnectedClientServer_RemoveClient_SyncProtectsRemoveClient	1126
7547618722826960891	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.TcpSocketPublisherTests.TwoConnectedClientsServer_Broadcast_SyncTwoEnqueingForBroadcast	1127
-8143904735275224676	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.UdpPublisherTests.ExplicitMultiCast_NewUdpPublisher_ResolvesIPFromExplicitAddress	1128
-1306018922829213234	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.UdpPublisherTests.NoMultiCastHost_NewUdpPublisher_ResolvesIPFromHostName	1129
3842414158465501632	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.UdpPublisherTests.BadMultiCastHost_NewUdpPublisher_ResolvesIPFromHostName	1130
3670611775151130911	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.UdpPublisherTests.StreamFromSubscriber_CreateAndConnect_FindsNicBroadcastIpCreatesSocket	1131
2029903199701682342	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.UdpPublisherTests.StreamFromSubscriber_CreateAndConnect_FindsNoNicBroadcastThrowsException	1132
5516171768802322427	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.UdpPublisherTests.StreamFromSubscriber_ConnectSetsConnector_UpdatesPublisherAcceptor	1133
8345614292201443968	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Publishing.UdpPublisherTests.StreamFromSubscriber_UdpPublisher_RefersBackToReferencingPublisher	1134
-8879581351573486110	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerFactoryTests.NewSocketDataLatencyLoggerFactory_Instance_CreatesNewInstance	1135
8605820832777887285	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerFactoryTests.NewSocketDataLatencyLoggerFactory_GetSocketLatencyLogger_ReturnsSameInstanceForSameKey	1136
-3966573541100699617	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.NewSocketDataLatencyLogger_New_InitializesCallStatLoggers	1137
3228696117376486889	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.NewSocketLatencyLogger_SettingTranslation_ParsesBatchSizeSettingsAsExpected	1138
-4996633728173344823	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.NewSocketLatencyLogger_DefaultStringValue_EqualsDisabled	1139
6676658122434186560	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.NewSocketLatencyLogger_Enabled_SetsBatchSizeToDefault	1140
971174339332451099	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.MockedSocketLatencyLogger_BatchSize_UsesAndSetsUnderlyingLoggersValues	1141
-7177800226208002436	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.SocketLatencyLogger_AddCallStat_ThrowsException	1142
307236824694356492	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.SocketLatencyLogger_AddCallStatTwoDates_ThrowsException	1143
2550114325481514518	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.SocketLatencyLogger_AddContextMeasurement_ThrowsException	1144
4637543035199056940	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.EnabledMultiSocketReceive_ParseTraceLog_CorrectTimingsSentToCallStatsForEachStage	1145
-6230743295456017513	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.EnabledNoDeserializerMultiSocketReceive_ParseTraceLog_CorrectTimingsSentToCallStatsForEachStage	1146
7192667341691898816	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.EnabledNotAbleToPublishMultiSocketReceive_ParseTraceLog_CorrectTimingsSentToCallStatsForEachStage	1147
5205638624216220967	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.DisabledSocketLatencyLoger_ParseTraceLog_IgnoresContentsLogsNothing	1148
-6565075019170510868	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Logging.SocketDataLatencyLoggerTests.Enabled_ParseTraceLogCausesException_LogsExceptionAndContinues	1149
3782058628936311715	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherBaseTests.NewDispatcherBase_New_SetsDispatcherDescription	1150
-3195672186131227094	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherBaseTests.UnstartedDispatcher_Start_IncrementsUsageCountLaunchesWorkerThreadAsExpected	1151
-5334509675145412683	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherBaseTests.StartedDispatcher_Start_IncrementsUsageCountDoesntRelaunchWorkerThread	1152
7016315370034764348	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherBaseTests.StartedDispatcherUsageCount1_Stop_DecrementsUsageCountStopsWorkThread	1153
6476892859771650470	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherBaseTests.StartedDispatcherUsageCount2_Stop_DecrementsUsageCountLeavesWorkerThreadRunning	1154
-3465740592744057548	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherListenerTests.NewDispatcherListenerWithDescription_New_ReceivesLatencyTraceCallsLoggerPool	1155
-3037517196981363286	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherListenerTests.NewDispatcherListener_RegisterForLister_RegistersSocketSessionConnectionWithSelector	1156
1114369319716674198	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherListenerTests.NewDispatcherListener_UnregisterForListen_UnregistersSocketSessionConnectionWithSelector	1157
5751062995806576286	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherListenerTests.SocketWithUpdate_ReceiveSuccessfullyCallsReceiveData_NormalProcessingOccurs	1158
236766866536675906	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherListenerTests.SocketWithUpdate_ReceiveDataReturnsFalse_ErrorIsLoggedAndConnectionErrorIsRaised	1159
-1944632089074046664	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherListenerTests.TwoSocketsWithUpdate_ReceiveFirstSocketThrowsException_SecondSocketIsProcessedNormally	1160
-7752258273955670937	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherListenerTests.SocketWithUpdate_SelectRecvThrowsException_RecoversAndCallsAgain	1161
-2928071068988228475	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherListenerTests.SocketWithUpdate_GetPerfLoggerThrowsException_RecoversAndCallsAgain	1162
4994927038042679865	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherListenerTests.AcceptorSocket_Receive_SocketSelectorReturnsSocketReceiveDataIsCalled	1163
8281097770071769022	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherSenderTests.NewDispatcherSender_New_CreatesOSThreadSignalSetsApplicationType	1164
-2960412080906415157	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherSenderTests.SocketSessionWithMessageToSend_AddToSendQueue_AddsSessionToWriteDictionary	1165
-8762168969870978127	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherSenderTests.TwoSocketSessionWithMessageToSend_AddToSendQueue_AddsSessionsToWriteDictionary	1166
2243133368614220796	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherSenderTests.ToWriteHasSocketSessionConnection_Send_CallsSendDataAndClearsWriting	1167
1715240283461579933	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherSenderTests.ToWriteHasSocketSessionConnection_SendDataReturnsFalse_RequeuesToWrite	1168
9219794399687963470	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherSenderTests.ToWriteHasTwoSocketSessionConnections_SendDataThrowsException_SecondSocketSendsSuccessfully	1169
-804832200965223130	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Dispatcher.SocketDispatcherSenderTests.ToWriteSocketSessionConnection_SocketNoLongerActive_SkipsCallToSendData	1170
-6364009762563688071	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Client.TcpSocketClientTests.KeepAlive_CreateAndConnect_ReturnsTCPOSSocket	1171
1816866295334011362	FortitudeTests: FortitudeTests.FortitudeIO.Transports.Sockets.Client.TcpSocketClientTests.NoKeepAlive_CreateAndConnect_ReturnsTCPOSSocket	1172
3503386237523232980	FortitudeTests: FortitudeTests.FortitudeIO.Protocols.Serialization.MessageDeserializerTests.RegisteredDeserializerWithCallbacks_Dispatch_LogsBeforePublishThenCallsRegisteredCallbacks	1173
-2297595275933006673	FortitudeTests: FortitudeTests.FortitudeIO.Protocols.ORX.Serialization.OrxByteSerializerTests.DoubleStringArrayType_Serializes_DeserializesIsSame	1174
-4392497747760791413	FortitudeTests: FortitudeTests.FortitudeIO.Protocols.ORX.Serialization.OrxByteSerializerTests.LongsType_Serializes_DeserializesIsSame	1175
-1780397812148861499	FortitudeTests: FortitudeTests.FortitudeIO.Protocols.ORX.Serialization.OrxByteSerializerTests.StringsType_Serializes_DeserializesIsSame	1176
-3264790370237890194	FortitudeTests: FortitudeTests.FortitudeIO.Protocols.ORX.Serialization.OrxByteSerializerTests.MutableStringsType_Serializes_DeserializesIsSame	1177
-4578522483913369008	FortitudeTests: FortitudeTests.FortitudeCommon.Serdes.Binary.ReadWriteBufferTests.ByteArray_New_SetsBufferAndDefaultsValues	1178
-8685499480915168784	FortitudeTests: FortitudeTests.FortitudeCommon.Serdes.Binary.ReadWriteBufferTests.WrittenBuffer_RemainingStorage_ReturnsExpectedValue_ResetWrittenReadToZero	1179
8991301413776457540	FortitudeTests: FortitudeTests.FortitudeCommon.Serdes.Binary.ReadWriteBufferTests.WrittenBuffer_MoveUnreadToBufferStart_ResetWrittenReadToZero	1180
7145918418903829447	FortitudeTests: FortitudeTests.FortitudeCommon.Monitoring.Logging.NLogAdapter.NLogFactoryTests.WritingNloggerClassCreatesExpectedFileWithOutput	1181
-662693263978555887	FortitudeTests: FortitudeTests.FortitudeCommon.Extensions.DiffNewUpdatedDeletedTests.ItemsWithId_Diffed_ShowCorrectUpdatedDeleted	1182
5179246638415980096	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Memory.GarbageAndLockFreePooledFactoryTests.MultipleElementsEnquedRemovingSomeElementsStillRetainsExistingItems	1183
364245053286702444	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Memory.GarbageAndLockFreePooledFactoryTests.MultipleReadersAndWritersEnqueingNoElementIsDoubleBooked	1184
-6089330072891960682	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Memory.GarbageAndLockFreePooledFactoryTests.SingleThreadAskingForMultipleItemsAndReturnsMultipleItems	1185
4974840309390420475	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Memory.StreamByteOpsTests.EmptyBuffer_MultipleStringEncodesThenDecodes_ReturnsMultipleStrings	1186
-6891234349198819717	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.ConcurrentMapTests.EmptyConcurrentMap_AddThenClear_SavesItemWithKeyClearRemovesAll	1187
-1081932165472392682	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.ConcurrentMapTests.ConcurrentMapWithItems_TryGetValue_FindsSomeOfTheItems	1188
-862757893900596786	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.ConcurrentMapTests.ConcurrentMapWithItems_Remove_LeavesRemainingItems	1189
4761756117214222146	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.ConcurrentMapTests.ConcurrentMapWithItems_Edit_FiresOnUpdateCallback	1190
-7575432177193452195	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.ConcurrentMapTests.ConcurrentMapWithItems_GetEnumerator_ReturnsMapValues	1191
-4702397548553202652	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.PopulatedLookup_ChangeInitializationDictionary_DoesnAffectLookup	1192
45264677980430591	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.EmptyLookup_New_ContainsNoData	1193
3878448300163266785	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	1194
-9023608126608041850	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.FromTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	1195
-4416009015091390470	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.OneMissingValue_AreEquivalent_ReturnsExpected	1196
7866436034146456517	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.OneDiffValue_AreEquivalent_ReturnsExpected	1197
-249521819635086411	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.ClonedPopulated_Equals_True	1198
4501916349915969181	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.PopulatedLookup_GetHashcode_IsNotZero	1199
2065468963683054448	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.EmptyLookup_GetHashcode_IsZero	1200
2495145267416411563	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.PopulatedNameId_GetEnumerator_IteratesKeyValuePairs	1201
2015705189481042425	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.IdLookupTests.PopulatedNameId_GenericGetEnumerator_IteratesKeyValuePairs	1202
-8971579562146055686	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.NameIdLookup_New_CanBeInitialized	1203
3703406014009492720	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.EmptyLookupGenerator_Indexer_AddsIfMissing	1204
7019895460183837475	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.GapPopulatedLookupGenerator_AddLowerIdEntry_NextEntryIsNextHighest	1205
-2813586467317074384	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.EmptyNameId_GetIdOrGetOrAddId_CreatesIdsIfNotAssigned	1206
-2428216348344006560	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.AddingSameItem_GetOrAddId_DoesNotAddNewIdOrThrowError	1207
6553941132847579211	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.PopulatedLookupGenerator_AppendNewNamesWithDifference_ThrowsExpection	1208
-1557936242763417275	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.PopulatedLookupGenerator_SetIdToNameWithDifferenceForId_ThrowsExpection	1209
7643520708485295897	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.PopulatedLookupGenerator_AddingKeyValueCollection_AddsNewItemsWIthSameId	1210
7056256915170179218	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.EmptyNameId_AddingKeyValueCollection_AddsNewItemsWIthSameId	1211
906041612081580848	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.EmptyNameIdLookup_CopyFromPopulatedLookupGenerator_CopiesValuesOver	1212
2255057613336132639	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.PopulatedIdLookupGenerator_CopyFromPopulatedLookupGenerator_DoesNothing	1213
7239633883044694886	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.PopulatedIdLookupGenerator_CopyFromSameInstance_DoesNothing	1214
3749794823157993345	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.PopulatedReadonlyIdLookup_CopyFromSameInstance_DoesNothing	1215
5190530051342802745	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	1216
4822238645918970741	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.FromBaseTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	1217
4695969812868151224	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.FromTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	1218
-522817394540157663	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.SmallerPopulatedLookupId_AreEquivalent_ReturnsExpected	1219
-2078955599824486635	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.LookupId_AreEquivalent_ReturnsFalse	1220
4171638246585978850	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupGeneratorTests.LookupIdGenerator_AreEquivalentExactTypes_ReturnsFalse	1221
1813219048352356292	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupTests.NameIdLookup_New_CanBeInitialized	1222
4554315221965242666	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupTests.GivenAnId_GetName_GetExpectedName	1223
-6833410158936175156	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupTests.FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	1224
-5296972718564795889	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupTests.FromBaseTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	1225
-1269317059936514769	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupTests.FromTypePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy	1226
-8468074403158276113	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupTests.LookupId_AreEquivalent_ReturnsFalse	1227
7447131582808425736	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupTests.LookupIdGenerator_AreEquivalentExactTypes_ReturnsFalse	1228
-1332485022344352573	FortitudeTests: FortitudeTests.FortitudeCommon.DataStructures.Maps.IdMap.NameIdLookupTests.ClonedPopulated_Equals_True	1229
5082681135190863564	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.ActionStateTimerCallBackRunInfoTests.CopyFromTest	1230
3437836429829108529	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.ActionStateTimerCallBackRunInfoTests.RunCallbackOnThreadPoolTest	1231
238092025138455498	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.ActionStateTimerCallBackRunInfoTests.RunCallbackOnThisThreadTest	1232
2849429986525720309	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.ActionTimerCallBackRunInfoTests.CopyFromTest	1233
1351459559490765744	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.ActionTimerCallBackRunInfoTests.RunCallbackOnThreadPoolTest	1234
4483650246607868747	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.ActionTimerCallBackRunInfoTests.RunCallbackOnThisThreadTest	1235
-2589841752501854624	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.IntervalTimerUpdateTests.Cancel	1236
2170570042553337662	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.IntervalTimerUpdateTests.UpdateWaitPeriod	1237
606864617872302992	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.IntervalTimerUpdateTests.PauseTest	1238
-2624186850303116539	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.IntervalTimerUpdateTests.ResumeTest	1239
-6857719881763129813	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.OneOffTimerUpdateTests.CopyFromTest	1240
-561277564324188454	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.OneOffTimerUpdateTests.IncrementAndDecrementRefCountTest	1241
691319980552543269	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.OneOffTimerUpdateTests.RecycleTest	1242
1275445145094723242	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.OneOffTimerUpdateTests.CancelTest	1243
-3295972363196954376	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.OneOffTimerUpdateTests.ExecuteNowOnThreadPoolTest	1244
-91721315894732797	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.OneOffTimerUpdateTests.ExecuteNowOnThisThreadTest	1245
3092181141534013781	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.OneOffTimerUpdateTests.UpdateWaitPeriodTest	1246
3693379232998328949	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.OneOffTimerUpdateTests.PauseTest	1247
-5922794219681273649	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.OneOffTimerUpdateTests.ResumeTest	1248
1300457086185047770	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerCallBackRunInfoTests.LowestNextScheduleTimeComparesLessThanOther	1249
-361911890771219027	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerCallBackRunInfoTests.CopyFromTest	1250
1925544013814049554	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerCallBackRunInfoTests.IncrementAndDecrementRefCountTest	1251
2634373055963632102	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerCallBackRunInfoTests.RecycleTest	1252
-665776924063090015	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.RunInExecutesExpectedNumberOfCallbacks	1253
-5136452061916394765	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.RunEveryIntMsStatelessWaitCallbackWorksAsExpected	1254
8897796258654925583	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.RunEveryIntMsActionCallbackWorksAsExpected	1255
-7384987120366478110	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.RunEveryIntMsStatefulWaitCallbackWorksAsExpected	1256
-5937758128597759100	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.RunAtExecutesExpectedNumberOfCallbacks	1257
-1416300145659030343	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.PauseAllThenResumeAllTimersStopsCallbacksThenResumesThem	1258
4258774145876756265	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.IndividualOneOffTimerUpdatePauseDoesNotAffectOtherRequests	1259
4223747915072276681	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.IndividualIntervalTimerUpdatePauseDoesNotAffectOtherRequests	1260
-8824255370276270199	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.OnOneOffTimerRunNextScheduledOneOffCallbackNowOnThisThread	1261
3177323779712560580	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.OnIntervalTimerRunNextScheduledOneOffCallbackNowOnThisThread	1262
3983000563565984390	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.OnOneOffTimerRunNextScheduledOneOffCallbackNowOnThreadPool	1263
-7000959205335803701	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.TimerTests.OnIntervalTimerRunNextScheduledOneOffCallbackNowOnThreadPool	1264
-5005933384339086461	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.WaitCallbackTimerCallBackRunInfoTests.CopyFromTest	1265
2698705444242427190	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.WaitCallbackTimerCallBackRunInfoTests.RunCallbackOnThreadPoolTest	1266
687857937432763853	FortitudeTests: FortitudeTests.FortitudeCommon.Chronometry.Timers.WaitCallbackTimerCallBackRunInfoTests.RunCallbackOnThisThreadTest	1267
-5597116519052833346	FortitudeTests: FortitudeTests.FortitudeCommon.AsyncProcessing.Tasks.ReusableValueTaskSourceTests.ReusableValueTaskSourceClearsTaskOnReset	1268
-2591488856581830582	FortitudeTests: FortitudeTests.FortitudeCommon.AsyncProcessing.Tasks.ReusableValueTaskSourceTests.ResetClearsCompletionsAddedOnPriorTaskAdditions	1269
-6544790822615003457	FortitudeTests: FortitudeTests.FortitudeCommon.AsyncProcessing.Tasks.ReusableValueTaskSourceTests.ReusableDecimalTaskIsReturnedToRecyclerAfterValueIsRead	1270
-2825558108184742708	FortitudeTests: FortitudeTests.FortitudeCommon.AsyncProcessing.Tasks.ReusableValueTaskSourceTests.ReusableObjTaskIsReturnedToRecyclerAfterValueIsRead	1271
1168994223344323518	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchDestinationCacheTests.SaveDispatchSenderDestinationWithNoExpiryButNotDestinationSavesJustExpectedEntry	1272
-2032319083738454373	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchDestinationCacheTests.SaveDispatchDestinationWithNoExpiryButNotSenderDestinationSavesJustExpectedEntry	1273
1796867783899292493	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchDestinationCacheTests.SaveDispatchBothDestinationAndSendDestinationWithNoExpirySavesBoth	1274
-3686252796771842845	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchDestinationCacheTests.SaveDispatchBothDestinationAndSendDestinationWith100ExpirySavesBothUntilRetrievedMoreThan100Times	1275
-2270158545202692171	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchDestinationCacheTests.SaveDispatchBothDestinationAndSendDestinationWith1MinExpirySavesBothUntilTimeExpires	1276
8730914225656170048	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchDestinationCacheTests.SaveDeployDestinationWithNoExpiryButNotCacheSenderDestinationSavesJustExpectedEntry	1277
-8215178805094666448	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.DefaultDeploySelectsRotateEvenlyLeastBusyAndFixedOrderSelectionStrategy	1278
-5291778478237688275	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.LeastBusyDeploySelectsLeastBusyAndFixedOrderSelectionStrategy	1279
1525258728227518028	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.NoFlagsDeploySelectsFixedOrderSelectionStrategy	1280
5453959019869486766	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.ReuseLastCacheDeploySelectsReuseLastCacheFixedOrderSelectionStrategy	1281
-7847209498562172930	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.TargetSpecificDeploySelectsTargetSpecificSelectionStrategy	1282
823917457326975110	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.DispatchDefaultPublishSelectsReuseLastCachedAndFixedOrderSelectionStrategy	1283
-1036781908657052566	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.DispatchDefaultRequestResponseSelectsRotateEvenlyFixedOrderSelectionStrategy	1284
7512737960426415814	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.DispatchPublishOneLeastBusyResponseSelectsLeastBusyFixedOrderSelectionStrategy	1285
-2550841874831921724	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.DispatchNoFlagsResponseSelectsFixedOrderSelectionStrategy	1286
-6338598668737218660	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DeployDispatchStrategySelectorTests.DispatchTargetSpecificResponseSelectsOnlyTargetSpecificSelectionStrategy	1287
-327137777713093119	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DispatchSelectionResultSetTests.OnlyUniqueEventQueuesAreSelected	1288
9150871031119415570	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DispatchSelectionResultSetTests.OnlyAcceptsUpToMaxUniqueResults	1289
-684804499359966456	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.DispatchSelectionResultSetTests.AddRangeOnlyAcceptsUpToMaxUniqueResults	1290
-6157280172931092825	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultPublishSelectCustomQueueTypeReturnsAllCustomQueues	1291
-2737519622774481858	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultRequestResponseSelectCustomQueueTypeReturnsFirstCustomQueue	1292
1179132302154905766	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultDeploySelectCustomQueueTypeReturnsFirstCustomQueue	1293
-3143703892714601300	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultPublishSelectWorkerQueueTypeReturnsAllWorkerQueues	1294
-6785070894776717730	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultRequestResponseSelectWorkerQueueTypeReturnsFirstWorkerQueue	1295
7748983039484611270	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultDeploySelectWorkerQueueTypeReturnsFirstWorkerQueue	1296
4952726493009253463	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultPublishSelectEventQueueTypeReturnsSecondEventQueue	1297
-1852319116449686914	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultRequestResponseSelectEventQueueTypeReturnsSecondEventQueue	1298
-8978907799821498318	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultDeploySelectEventQueueTypeReturnsSecondEventQueue	1299
7959537034108505793	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultPublishSelectIoInboundTypeReturnsFirstIoInboundQueue	1300
-141851571656334866	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultRequestResponseSelectIoInboundQueueTypeReturnsFirstIoInboundQueue	1301
7626764079958247388	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultDeploySelectIoInboundQueueTypeReturnsFirstIoInboundQueue	1302
7836664940262010163	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultPublishSelectIoOutboundTypeReturnsFirstIoOutboundQueue	1303
4776222105303431085	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultRequestResponseSelectIoOutboundQueueTypeReturnsFirstIoOutboundQueue	1304
2104993403377692519	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.DefaultDeploySelectIoOutboundQueueTypeReturnsFirstIoOutboundQueue	1305
8524398317037426129	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.NoneEventQueueDispatchRuleReturnsEmptySelectionResults	1306
-3020259999343376675	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.FixedOrderSelectionStrategyTests.NoneEventQueueDeployRuleReturnsNull	1307
-6410872747388853356	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyPublishOneOrRequestResponseSelectCustomQueueTypeReturnsSecondCustomQueue	1308
-2734078166033749347	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyDeploySelectCustomQueueTypeReturnsSecondCustomQueue	1309
696890939607808317	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyPublishOneOrRequestResponseSelectWorkerQueueTypeReturnsFirstWorkerQueue	1310
-5248779109464195655	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyDeploySelectWorkerQueueTypeReturnsFirstWorkerQueue	1311
3635981740501520847	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyPublishOneOrRequestResponseSelectEventQueueTypeReturnsSecondEventQueue	1312
7868981472740619149	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyDeploySelectEventQueueTypeReturnsSecondEventQueue	1313
-5382392792071956320	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyPublishOneOrRequestResponseSelectIoInboundTypeReturnsFirstIoInboundQueue	1314
4722228480134289451	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyDeploySelectIoInboundQueueTypeReturnsFirstIoInboundQueue	1315
2258045652370874348	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyPublishOneOrRequestResponseSelectIoOutboundTypeReturnsSecondIoOutboundQueue	1316
7458238141391050739	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusyDeploySelectIoOutboundQueueTypeReturnsSecondIoOutboundQueue	1317
5324069311841853375	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.LeastBusySelectNoEventQueueDispatchRuleReturnsEmptySelectionResults	1318
7682350723153265293	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.LeastBusySelectionStrategyTests.NoneEventQueueDeployRuleReturnsNull	1319
-255479049949798456	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DispatchDefaultPublishPreviouslyCachedResultIsReturnedUntilExpiredAfter100Requests	1320
8244428772910906594	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DispatchDefaultPublishPreviouslyCachedResultIsReturnedUntilExpiredAfter1Min	1321
7802840229146774501	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DispatchPublishNoExpiryPreviouslyCachedResultIsReturnedIndefinitely	1322
2672924392723299654	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DispatchPublishNoExpiryPreviouslyCachedResultIsReturnedUnlessRecalculateCacheIsSent	1323
3420410409558794600	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DispatchDefaultRequestResponseWithDifferingSendersRetainsCachedResultForEachSender	1324
-1879805963535627571	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DispatchDefaultRequestResponseDiffDestinationAddressCachesResultForEachDestination	1325
7902977540104998009	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DefaultDeployPreviouslyCachedResultIsReturnedIndefinitely	1326
987872124572520775	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DefaultDeployWithDifferingQueuesGroupsRetainPreviousCacheStatus	1327
2486188709265545329	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DeployWithExpiryAfter100ReadsPreviouslyCachedResultIsReturnedUntilItExpires	1328
-7223695806028118107	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.ReuseLastCachedResultSelectionStrategyTests.DeployWithExpiryAfter1MinPreviouslyCachedResultIsReturnedUntilItExpires	1329
-952303128732388034	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyPublishOneOrRequestResponseSelectCustomQueueTypeReturnsSecondThirdFirstCustomQueue	1330
6220656556765335703	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyDeploySelectCustomQueueTypeReturnsSecondThirdAndFirstCustomQueue	1331
-7168421973051444652	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyPublishOneOrRequestResponseSelectWorkerQueueTypeReturnsOnly3WorkerQueueRepeatedly	1332
-7929991792431856222	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyDeploySelectWorkerQueueTypeReturns2_3_1WorkerQueue	1333
-3009228473796984114	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyPublishOneOrRequestResponseSelectEventQueueTypeReturns2_3_3_1EventQueue	1334
7716124063300007096	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyDeploySelectEventQueueTypeReturns2_3_1EventQueue	1335
5536513000467519387	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyPublishOneOrRequestResponseSelectIoInboundTypeReturnsFirstIoInboundQueueRepeatedly	1336
2628421056183551099	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyDeploySelectIoInboundQueueTypeReturnsReturns2_3_1IoInboundQueue	1337
-92720985729499244	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyPublishOneOrRequestResponseSelectIoOutboundTypeReturns3_2_3IoOutboundQueue	1338
-8226364803093571800	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlyDeploySelectIoOutboundQueueTypeReturns2_3_1IoOutboundQueue	1339
-7741358946032307142	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.RotateEvenlySelectNoCacheQueueDispatchRuleReturnsNull	1340
6920604210740085850	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RotateEvenlySelectionStrategyTests.NoneEventQueueDeployRuleReturnsNull	1341
-1483961530525751665	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.SelectionStrategiesAggregatorTests.DefaultPublishWithCustomQueueTypeFixedOrderSelectionStrategyReturnsFirstCustomQueue	1342
6213761762685107727	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.SelectionStrategiesAggregatorTests.DefaultDeployCustomEventQueueTypeFixedOrderStrategyReturnsFirstCustomQueue	1343
-7601845516294979005	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.SelectionStrategiesAggregatorTests.DefaultRequestResponseSelectEventQueueTypeReturnsEventQueue	1344
-8937175801598297559	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.SelectionStrategiesAggregatorTests.WithOrderSelectionStrategyNoneEventQueueRuleReturnsEmptySelectionResults	1345
2640566765996170294	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.SelectionStrategiesAggregatorTests.WithOrderSelectionStrategyNoneEventQueueDeployRuleReturnsNull	1346
8144422378637737611	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingFlagsExtensionsTests.XorToggleFlagsCorrectlyTogglesFlagsWhenXorWithOriginal	1347
8023933319764954538	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingFlagsExtensionsTests.XorToggleFlagsWithNoEnabledCorrectlyTogglesFlagsWhenXorWithOriginal	1348
-8310467304190011765	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingFlagsExtensionsTests.XorToggleFlagsWithNoDisabledCorrectlyTogglesFlagsWhenXorWithOriginal	1349
5122595439880943005	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.RoutingFlagsExtensionsTests.XorToggleFlagsNoAlterationsReturnsOriginal	1350
-7367917403558354336	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.SwitchingBroadcastSenderDestinationCacheTests.DefaultPublishAllResultsDoesNotAffectDefaultRequestResponseWithSenderCacheLastForTheSameDestination	1351
8409376315647180451	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.TargetSpecificSelectionStrategyTests.DispatchTargetSpecificRuleFindsEventQueueInAllQueuesAndReturnsExpectedResult	1352
-9085754636720657457	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.TargetSpecificSelectionStrategyTests.DispatchTargetSpecificRuleThatIsNotStartedReturnsNoResult	1353
-614415171202939901	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.TargetSpecificSelectionStrategyTests.DispatchTargetSpecificRuleThatIsOnEventQueueInAvailableQueuesReturnsNoResult	1354
-2457224510879870735	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.TargetSpecificSelectionStrategyTests.DispatchNoSetTargetSpecificRuleReturnsNoResult	1355
-5248650230956437074	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.TargetSpecificSelectionStrategyTests.DeployTargetSpecificQueueNameRuleReturnsExpectedResult	1356
3638941178979011375	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.TargetSpecificSelectionStrategyTests.DeployTargetSpecificQueueNameThatCantBeFoundRuleReturnsNull	1357
9116659678081060652	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Routing.SelectionStrategies.TargetSpecificSelectionStrategyTests.DeployTargetSpecificWithNoQueueNameReturnsNull	1358
7943931625662181984	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Pipelines.EventQueueTests.EventQueueCanLoadNewRuleAndRunStart	1359
7765117299931483029	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Pipelines.EventQueueTests.StartingListeningRuleThenPublishingRuleExpectToReceiveSamePublishedMessages	1360
-761294447628058446	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Pipelines.EventQueueTests.StartResponderThenStartRequesterEachReceiveExpectedNumberOfInvocations	1361
6057698298866825827	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Pipelines.EventQueueTests.StartResponderThenAsyncValueTaskResponderThenStartRequesterEachReceiveExpectedNumberOfInvocations	1362
5222284899394960018	FortitudeTests: FortitudeTests.FortitudeBusRules.MessageBus.Pipelines.EventQueueTests.StartResponderThenAsyncTaskResponderThenStartRequesterEachReceiveExpectedNumberOfInvocations	1363
\.


--
-- TOC entry 6301 (class 0 OID 16735)
-- Dependencies: 263
-- Data for Name: user_attribute; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_attribute (user_id, attr_key, attr_value) FROM stdin;
\.


--
-- TOC entry 6302 (class 0 OID 16742)
-- Dependencies: 264
-- Data for Name: user_blocks; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_blocks (user_id, block_type, state) FROM stdin;
\.


--
-- TOC entry 6377 (class 0 OID 17269)
-- Dependencies: 339
-- Data for Name: user_build_parameters; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_build_parameters (build_state_id, param_name, param_value) FROM stdin;
\.


--
-- TOC entry 6321 (class 0 OID 16869)
-- Dependencies: 283
-- Data for Name: user_build_types_order; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_build_types_order (user_id, project_int_id, bt_int_id, ordernum, visible) FROM stdin;
\.


--
-- TOC entry 6305 (class 0 OID 16761)
-- Dependencies: 267
-- Data for Name: user_notification_data; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_notification_data (user_id, rule_id, additional_data) FROM stdin;
\.


--
-- TOC entry 6303 (class 0 OID 16749)
-- Dependencies: 265
-- Data for Name: user_notification_events; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_notification_events (id, user_id, notificator_type, events_mask) FROM stdin;
\.


--
-- TOC entry 6320 (class 0 OID 16863)
-- Dependencies: 282
-- Data for Name: user_projects_order; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_projects_order (user_id, project_int_id, ordernum) FROM stdin;
\.


--
-- TOC entry 6319 (class 0 OID 16857)
-- Dependencies: 281
-- Data for Name: user_projects_visibility; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_projects_visibility (user_id, project_int_id, visible) FROM stdin;
1	project1	1
\.


--
-- TOC entry 6300 (class 0 OID 16727)
-- Dependencies: 262
-- Data for Name: user_property; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_property (user_id, prop_key, prop_value, locase_value_hash) FROM stdin;
1	teamcity.server.buildNumber	147512	1452542498
1	addTriggeredBuildToFavorites	true	3569038
1	was.logged.in	true	3569038
1	hasSeenExperimentalOverview	true	3569038
1	lastSeenSakuraUIVersion	2023.11.3	506728308
1	lastSelectedCreateObjectOption	createFromUrl	-12443383
1	visible.projects.configured	true	3569038
1	showAdvancedOpts_vcsRootSettings_jetbrains.git	true	3569038
1	showAllBuildTypeTabs	true	3569038
1	showAdvancedOpts_buildTypeVcsSettings	true	3569038
1	showAllProjectTabs	true	3569038
1	showAdvancedOpts_buildTypeGeneralSettings	true	3569038
1	showAdvancedOpts_editCreateProject	true	3569038
\.


--
-- TOC entry 6384 (class 0 OID 17318)
-- Dependencies: 346
-- Data for Name: user_roles; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_roles (user_id, role_id, project_int_id) FROM stdin;
-1000	PROJECT_VIEWER	\N
1	SYSTEM_ADMIN	\N
\.


--
-- TOC entry 6304 (class 0 OID 16756)
-- Dependencies: 266
-- Data for Name: user_watch_type; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.user_watch_type (rule_id, user_id, notificator_type, watch_type, watch_value, order_num) FROM stdin;
\.


--
-- TOC entry 6310 (class 0 OID 16791)
-- Dependencies: 272
-- Data for Name: usergroup_notification_data; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.usergroup_notification_data (group_id, rule_id, additional_data) FROM stdin;
ALL_USERS_GROUP	1	userChangesOnly='true'
ALL_USERS_GROUP	2	userChangesOnly='true'
\.


--
-- TOC entry 6308 (class 0 OID 16779)
-- Dependencies: 270
-- Data for Name: usergroup_notification_events; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.usergroup_notification_events (id, group_id, notificator_type, events_mask) FROM stdin;
1	ALL_USERS_GROUP	email	8260
2	ALL_USERS_GROUP	IDE_Notificator	76
3	ALL_USERS_GROUP	email	512
4	ALL_USERS_GROUP	IDE_Notificator	512
5	ALL_USERS_GROUP	WindowsTray	512
6	ALL_USERS_GROUP	jabber	512
\.


--
-- TOC entry 6298 (class 0 OID 16711)
-- Dependencies: 260
-- Data for Name: usergroup_property; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.usergroup_property (group_id, prop_key, prop_value) FROM stdin;
\.


--
-- TOC entry 6385 (class 0 OID 17324)
-- Dependencies: 347
-- Data for Name: usergroup_roles; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.usergroup_roles (group_id, role_id, project_int_id) FROM stdin;
ALL_USERS_GROUP	PROJECT_DEVELOPER	\N
\.


--
-- TOC entry 6306 (class 0 OID 16769)
-- Dependencies: 268
-- Data for Name: usergroup_subgroups; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.usergroup_subgroups (hostgroup_id, subgroup_id) FROM stdin;
\.


--
-- TOC entry 6307 (class 0 OID 16774)
-- Dependencies: 269
-- Data for Name: usergroup_users; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.usergroup_users (group_id, user_id) FROM stdin;
\.


--
-- TOC entry 6309 (class 0 OID 16786)
-- Dependencies: 271
-- Data for Name: usergroup_watch_type; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.usergroup_watch_type (rule_id, group_id, notificator_type, watch_type, watch_value, order_num) FROM stdin;
1	ALL_USERS_GROUP	email	2	_Root	1
2	ALL_USERS_GROUP	IDE_Notificator	2	_Root	1
3	ALL_USERS_GROUP	email	5	__SYSTEM_WIDE__	2
4	ALL_USERS_GROUP	IDE_Notificator	5	__SYSTEM_WIDE__	2
5	ALL_USERS_GROUP	WindowsTray	5	__SYSTEM_WIDE__	1
6	ALL_USERS_GROUP	jabber	5	__SYSTEM_WIDE__	1
\.


--
-- TOC entry 6297 (class 0 OID 16702)
-- Dependencies: 259
-- Data for Name: usergroups; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.usergroups (group_id, name, description) FROM stdin;
ALL_USERS_GROUP	All Users	Contains all TeamCity users
\.


--
-- TOC entry 6299 (class 0 OID 16718)
-- Dependencies: 261
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.users (id, username, password, name, email, last_login_timestamp, algorithm) FROM stdin;
1	fortitude	$2a$07$Du8wnLrn24iHgPlyhA/ARuVzsDb6HlDPAgYfHFyBE2Ab78dj8Dms2			1710285396764	BCRYPT
\.


--
-- TOC entry 6325 (class 0 OID 16897)
-- Dependencies: 287
-- Data for Name: vcs_change; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_change (modification_id, file_num, vcs_file_name, vcs_file_name_hash, relative_file_name_pos, relative_file_name, relative_file_name_hash, change_type, change_name, before_revision, after_revision) FROM stdin;
1	1	cicd/.gitignore	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	2	cicd/Readme.txt	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	3	cicd/TeamcityUrl.url	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	4	cicd/start-teamcity-docker.bat	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	5	cicd/teamcity/.gitignore	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	6	cicd/teamcity/config/config/_auth/auth-preset.dtd	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	7	cicd/teamcity/config/config/_auth/default.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	8	cicd/teamcity/config/config/_auth/default.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	9	cicd/teamcity/config/config/_auth/ldap-ntlm.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	10	cicd/teamcity/config/config/_auth/ldap-ntlm.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	11	cicd/teamcity/config/config/_auth/ldap.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	12	cicd/teamcity/config/config/_auth/ldap.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	13	cicd/teamcity/config/config/_auth/nt-domain.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	14	cicd/teamcity/config/config/_auth/nt-domain.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	15	cicd/teamcity/config/config/_logging/debug-ClearCase.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	16	cicd/teamcity/config/config/_logging/debug-ClearCase.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	17	cicd/teamcity/config/config/_logging/debug-SVN.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	18	cicd/teamcity/config/config/_logging/debug-SVN.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	19	cicd/teamcity/config/config/_logging/debug-all.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	20	cicd/teamcity/config/config/_logging/debug-all.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	21	cicd/teamcity/config/config/_logging/debug-artifacts.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	22	cicd/teamcity/config/config/_logging/debug-artifacts.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	23	cicd/teamcity/config/config/_logging/debug-auth.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	24	cicd/teamcity/config/config/_logging/debug-auth.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	25	cicd/teamcity/config/config/_logging/debug-cleanup.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	26	cicd/teamcity/config/config/_logging/debug-cleanup.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	27	cicd/teamcity/config/config/_logging/debug-cloud.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	28	cicd/teamcity/config/config/_logging/debug-cloud.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	29	cicd/teamcity/config/config/_logging/debug-commit-status.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	30	cicd/teamcity/config/config/_logging/debug-commit-status.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	31	cicd/teamcity/config/config/_logging/debug-flaky-tests.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	32	cicd/teamcity/config/config/_logging/debug-flaky-tests.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	33	cicd/teamcity/config/config/_logging/debug-general.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	34	cicd/teamcity/config/config/_logging/debug-general.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	35	cicd/teamcity/config/config/_logging/debug-issue-trackers.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	36	cicd/teamcity/config/config/_logging/debug-issue-trackers.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	37	cicd/teamcity/config/config/_logging/debug-ldap.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	38	cicd/teamcity/config/config/_logging/debug-ldap.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	39	cicd/teamcity/config/config/_logging/debug-nodes.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	40	cicd/teamcity/config/config/_logging/debug-nodes.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	41	cicd/teamcity/config/config/_logging/debug-notifications.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	42	cicd/teamcity/config/config/_logging/debug-notifications.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	43	cicd/teamcity/config/config/_logging/debug-pull-requests.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	44	cicd/teamcity/config/config/_logging/debug-pull-requests.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	45	cicd/teamcity/config/config/_logging/debug-rest.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	46	cicd/teamcity/config/config/_logging/debug-rest.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	47	cicd/teamcity/config/config/_logging/debug-search.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	48	cicd/teamcity/config/config/_logging/debug-search.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	49	cicd/teamcity/config/config/_logging/debug-sql.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	50	cicd/teamcity/config/config/_logging/debug-sql.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	51	cicd/teamcity/config/config/_logging/debug-triggers.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	52	cicd/teamcity/config/config/_logging/debug-triggers.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	53	cicd/teamcity/config/config/_logging/debug-vcs.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	54	cicd/teamcity/config/config/_logging/debug-vcs.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	55	cicd/teamcity/config/config/_logging/debug-versioned-settings.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	56	cicd/teamcity/config/config/_logging/debug-versioned-settings.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	57	cicd/teamcity/config/config/_logging/debug-ws.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	58	cicd/teamcity/config/config/_logging/debug-ws.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	59	cicd/teamcity/config/config/_logging/debug-xml-rpc.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	60	cicd/teamcity/config/config/_logging/debug-xml-rpc.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	61	cicd/teamcity/config/config/_logging/log4j.dtd	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	62	cicd/teamcity/config/config/_notifications/email/build_failed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	63	cicd/teamcity/config/config/_notifications/email/build_failed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	64	cicd/teamcity/config/config/_notifications/email/build_failed_to_start.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	65	cicd/teamcity/config/config/_notifications/email/build_failed_to_start.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	66	cicd/teamcity/config/config/_notifications/email/build_failing.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	67	cicd/teamcity/config/config/_notifications/email/build_failing.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	68	cicd/teamcity/config/config/_notifications/email/build_probably_hanging.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	69	cicd/teamcity/config/config/_notifications/email/build_probably_hanging.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	70	cicd/teamcity/config/config/_notifications/email/build_problem_responsibility_assigned_to_me.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	71	cicd/teamcity/config/config/_notifications/email/build_problem_responsibility_assigned_to_me.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	72	cicd/teamcity/config/config/_notifications/email/build_problem_responsibility_changed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	73	cicd/teamcity/config/config/_notifications/email/build_problem_responsibility_changed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	74	cicd/teamcity/config/config/_notifications/email/build_problems_muted.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	75	cicd/teamcity/config/config/_notifications/email/build_problems_muted.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	76	cicd/teamcity/config/config/_notifications/email/build_problems_unmuted.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	77	cicd/teamcity/config/config/_notifications/email/build_problems_unmuted.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	78	cicd/teamcity/config/config/_notifications/email/build_started.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	79	cicd/teamcity/config/config/_notifications/email/build_started.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	80	cicd/teamcity/config/config/_notifications/email/build_successful.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	81	cicd/teamcity/config/config/_notifications/email/build_successful.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	82	cicd/teamcity/config/config/_notifications/email/build_type_responsibility_assigned_to_me.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	83	cicd/teamcity/config/config/_notifications/email/build_type_responsibility_assigned_to_me.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	84	cicd/teamcity/config/config/_notifications/email/build_type_responsibility_changed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	85	cicd/teamcity/config/config/_notifications/email/build_type_responsibility_changed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	86	cicd/teamcity/config/config/_notifications/email/common.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	87	cicd/teamcity/config/config/_notifications/email/common.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	88	cicd/teamcity/config/config/_notifications/email/email-config.dtd	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	89	cicd/teamcity/config/config/_notifications/email/email-config.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	90	cicd/teamcity/config/config/_notifications/email/email_verification.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	91	cicd/teamcity/config/config/_notifications/email/email_verification.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	92	cicd/teamcity/config/config/_notifications/email/labeling_failed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	93	cicd/teamcity/config/config/_notifications/email/labeling_failed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	94	cicd/teamcity/config/config/_notifications/email/multiple_test_responsibility_assigned_to_me.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	95	cicd/teamcity/config/config/_notifications/email/multiple_test_responsibility_assigned_to_me.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	96	cicd/teamcity/config/config/_notifications/email/multiple_test_responsibility_changed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	97	cicd/teamcity/config/config/_notifications/email/multiple_test_responsibility_changed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	98	cicd/teamcity/config/config/_notifications/email/mute.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	99	cicd/teamcity/config/config/_notifications/email/mute.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	100	cicd/teamcity/config/config/_notifications/email/queued_build_waiting_approval.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	101	cicd/teamcity/config/config/_notifications/email/queued_build_waiting_approval.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	102	cicd/teamcity/config/config/_notifications/email/reset_password.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	103	cicd/teamcity/config/config/_notifications/email/reset_password.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	104	cicd/teamcity/config/config/_notifications/email/responsibility.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	105	cicd/teamcity/config/config/_notifications/email/responsibility.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	106	cicd/teamcity/config/config/_notifications/email/test_responsibility_assigned_to_me.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	107	cicd/teamcity/config/config/_notifications/email/test_responsibility_assigned_to_me.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	108	cicd/teamcity/config/config/_notifications/email/test_responsibility_changed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	109	cicd/teamcity/config/config/_notifications/email/test_responsibility_changed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	110	cicd/teamcity/config/config/_notifications/email/tests_muted.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	111	cicd/teamcity/config/config/_notifications/email/tests_muted.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	112	cicd/teamcity/config/config/_notifications/email/tests_unmuted.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	113	cicd/teamcity/config/config/_notifications/email/tests_unmuted.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	114	cicd/teamcity/config/config/_notifications/ide_notificator/build_failed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	115	cicd/teamcity/config/config/_notifications/ide_notificator/build_failed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	116	cicd/teamcity/config/config/_notifications/ide_notificator/build_failed_to_start.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	117	cicd/teamcity/config/config/_notifications/ide_notificator/build_failed_to_start.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	118	cicd/teamcity/config/config/_notifications/ide_notificator/build_failing.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	119	cicd/teamcity/config/config/_notifications/ide_notificator/build_failing.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	120	cicd/teamcity/config/config/_notifications/ide_notificator/build_probably_hanging.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	121	cicd/teamcity/config/config/_notifications/ide_notificator/build_probably_hanging.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	122	cicd/teamcity/config/config/_notifications/ide_notificator/build_started.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	123	cicd/teamcity/config/config/_notifications/ide_notificator/build_started.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	124	cicd/teamcity/config/config/_notifications/ide_notificator/build_successful.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	125	cicd/teamcity/config/config/_notifications/ide_notificator/build_successful.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	126	cicd/teamcity/config/config/_notifications/ide_notificator/build_type_responsibility_assigned_to_me.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	127	cicd/teamcity/config/config/_notifications/ide_notificator/build_type_responsibility_assigned_to_me.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	128	cicd/teamcity/config/config/_notifications/ide_notificator/build_type_responsibility_changed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	129	cicd/teamcity/config/config/_notifications/ide_notificator/build_type_responsibility_changed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	130	cicd/teamcity/config/config/_notifications/ide_notificator/common.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	131	cicd/teamcity/config/config/_notifications/ide_notificator/common.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	132	cicd/teamcity/config/config/_notifications/ide_notificator/labeling_failed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	133	cicd/teamcity/config/config/_notifications/ide_notificator/labeling_failed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	134	cicd/teamcity/config/config/_notifications/ide_notificator/multiple_test_responsibility_assigned_to_me.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	135	cicd/teamcity/config/config/_notifications/ide_notificator/multiple_test_responsibility_assigned_to_me.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	136	cicd/teamcity/config/config/_notifications/ide_notificator/multiple_test_responsibility_changed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	137	cicd/teamcity/config/config/_notifications/ide_notificator/multiple_test_responsibility_changed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	138	cicd/teamcity/config/config/_notifications/ide_notificator/mute.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	139	cicd/teamcity/config/config/_notifications/ide_notificator/mute.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	140	cicd/teamcity/config/config/_notifications/ide_notificator/responsibility.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	141	cicd/teamcity/config/config/_notifications/ide_notificator/responsibility.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	142	cicd/teamcity/config/config/_notifications/ide_notificator/test_responsibility_assigned_to_me.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	143	cicd/teamcity/config/config/_notifications/ide_notificator/test_responsibility_assigned_to_me.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	144	cicd/teamcity/config/config/_notifications/ide_notificator/test_responsibility_changed.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	145	cicd/teamcity/config/config/_notifications/ide_notificator/test_responsibility_changed.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	146	cicd/teamcity/config/config/_notifications/ide_notificator/tests_muted.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	147	cicd/teamcity/config/config/_notifications/ide_notificator/tests_muted.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	148	cicd/teamcity/config/config/_notifications/ide_notificator/tests_unmuted.ftl	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	149	cicd/teamcity/config/config/_notifications/ide_notificator/tests_unmuted.ftl.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	150	cicd/teamcity/config/config/auth-config.dtd	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	151	cicd/teamcity/config/config/auth-config.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	152	cicd/teamcity/config/config/backup-config.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	153	cicd/teamcity/config/config/build-queue-priorities.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	154	cicd/teamcity/config/config/change-viewers.properties.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	155	cicd/teamcity/config/config/database.hsqldb.properties.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	156	cicd/teamcity/config/config/database.mssql.properties.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	157	cicd/teamcity/config/config/database.mysql.properties.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	158	cicd/teamcity/config/config/database.oracle.properties.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	159	cicd/teamcity/config/config/database.postgresql.properties.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	160	cicd/teamcity/config/config/database.properties	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	161	cicd/teamcity/config/config/disabled-plugins.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	162	cicd/teamcity/config/config/ldap-config.properties.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	163	cicd/teamcity/config/config/ldap-mapping.dtd	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	164	cicd/teamcity/config/config/ldap-mapping.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	165	cicd/teamcity/config/config/main-config.dtd	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	166	cicd/teamcity/config/config/main-config.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	167	cicd/teamcity/config/config/nodes-config.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	168	cicd/teamcity/config/config/ntlm-config.properties	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	169	cicd/teamcity/config/config/projects/Fortitude/buildTypes/Fortitude_Build.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	170	cicd/teamcity/config/config/projects/Fortitude/buildTypes/Fortitude_Build.xml.1	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	171	cicd/teamcity/config/config/projects/Fortitude/buildTypes/Fortitude_Build.xml.2	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	172	cicd/teamcity/config/config/projects/Fortitude/buildTypes/Fortitude_Build.xml.3	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	173	cicd/teamcity/config/config/projects/Fortitude/pluginData/.gitignore	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	174	cicd/teamcity/config/config/projects/Fortitude/project-config.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	175	cicd/teamcity/config/config/projects/Fortitude/project-config.xml.1	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	176	cicd/teamcity/config/config/projects/Fortitude/vcsRoots/Fortitude_SshGitGithubComShwaindogFortitudeGitRefsHeadsMain.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	177	cicd/teamcity/config/config/projects/Fortitude/vcsRoots/Fortitude_SshGitGithubComShwaindogFortitudeGitRefsHeadsMain.xml.1	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	178	cicd/teamcity/config/config/projects/_Root/pluginData/plugin-settings.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	179	cicd/teamcity/config/config/projects/_Root/project-config.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	180	cicd/teamcity/config/config/roles-config.dtd	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	181	cicd/teamcity/config/config/roles-config.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	182	cicd/teamcity/config/config/roles-config.xml.dist	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	183	cicd/teamcity/config/config/usage-statistics-config.xml	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	184	cicd/teamcity/config/lib/jdbc/postgresql-42.5.0.jar	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	185	cicd/teamcity/config/plugins/.tools/.gitignore	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	186	cicd/teamcity/config/plugins/.tools/NuGet.CommandLine.6.8.0.nupkg	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	187	cicd/teamcity/config/plugins/teamcity-commit-hooks.zip	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	188	cicd/teamcity/config/system/.gitignore	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	189	cicd/teamcity/config/system/pluginData/.gitignore	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	190	cicd/teamcity/config/system/pluginData/async-event-dispatcher/unprocessedAsyncEvents.bak	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	191	cicd/teamcity/config/system/pluginData/commit-hooks/webhooks.json	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	192	cicd/teamcity/config/system/pluginData/healthReports/xmxWarningWasShown	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	193	cicd/teamcity/config/system/pluginData/versionedSettings/version	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	194	src/ForitudeBusRules/ForititudeBusRules.csproj	\N	0	\N	\N	0	edited	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	195	src/ForitudeBusRules/NugetReadme.md	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	196	src/ForitudeBusRules/ReleaseNotes.md	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	197	src/Fortitude.sln	\N	0	\N	\N	0	edited	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	198	src/FortitudeCommon/FortitudeCommon.csproj	\N	0	\N	\N	0	edited	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	199	src/FortitudeIO/FortitudeIO.csproj	\N	0	\N	\N	0	edited	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	200	src/FortitudeMarketsApi/FortitudeMarketsApi.csproj	\N	0	\N	\N	0	edited	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	201	src/FortitudeMarketsCore/FortitudeMarketsCore.csproj	\N	0	\N	\N	0	edited	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
1	202	wingedHelmet.jpg	\N	0	\N	\N	1	added	c264c358828fb6491475be79820450fb19bc225c	cc625b80fa8d68a300397d498553649485c70862
2	1	cicd/Readme.txt	\N	0	\N	\N	0	edited	cc625b80fa8d68a300397d498553649485c70862	01be4c770e3295ab944eefdb9510511e7987b74e
2	2	cicd/start-teamcity-docker.bat	\N	0	\N	\N	0	edited	cc625b80fa8d68a300397d498553649485c70862	01be4c770e3295ab944eefdb9510511e7987b74e
2	3	cicd/teamcity/config/config/projects/Fortitude/buildTypes/Fortitude_Build.xml	\N	0	\N	\N	0	edited	cc625b80fa8d68a300397d498553649485c70862	01be4c770e3295ab944eefdb9510511e7987b74e
2	4	src/FortitudeTests/FortitudeMarketsCore/Pricing/PQ/Serialization/Deserialization/SyncState/SynchronisingStateTests.cs	\N	0	\N	\N	0	edited	cc625b80fa8d68a300397d498553649485c70862	01be4c770e3295ab944eefdb9510511e7987b74e
\.


--
-- TOC entry 6329 (class 0 OID 16929)
-- Dependencies: 291
-- Data for Name: vcs_change_attrs; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_change_attrs (modification_id, attr_name, attr_value) FROM stdin;
\.


--
-- TOC entry 6413 (class 0 OID 17502)
-- Dependencies: 375
-- Data for Name: vcs_changes; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_changes (modification_id, change_name, change_type, before_revision, after_revision, vcs_file_name, relative_file_name) FROM stdin;
\.


--
-- TOC entry 6328 (class 0 OID 16923)
-- Dependencies: 290
-- Data for Name: vcs_changes_graph; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_changes_graph (child_modification_id, child_revision, parent_num, parent_modification_id, parent_revision) FROM stdin;
1	cc625b80fa8d68a300397d498553649485c70862	0	\N	c264c358828fb6491475be79820450fb19bc225c
2	01be4c770e3295ab944eefdb9510511e7987b74e	0	1	cc625b80fa8d68a300397d498553649485c70862
\.


--
-- TOC entry 6324 (class 0 OID 16888)
-- Dependencies: 286
-- Data for Name: vcs_history; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_history (modification_id, user_name, description, change_date, register_date, vcs_root_id, changes_count, version, display_version) FROM stdin;
1	shwaindog	Adding Teamcity /data/teamcity_server/datadir folder and docker launch command with instructions\n	1710066153000	1710285417766	2	202	cc625b80fa8d68a300397d498553649485c70862	cc625b80fa8d68a300397d498553649485c70862
2	shwaindog	Add teamcity server launch docker bat file\n	1710289178000	1710289234127	2	4	01be4c770e3295ab944eefdb9510511e7987b74e	01be4c770e3295ab944eefdb9510511e7987b74e
\.


--
-- TOC entry 6280 (class 0 OID 16595)
-- Dependencies: 242
-- Data for Name: vcs_root; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_root (int_id, config_id, origin_project_id, delete_time) FROM stdin;
-1	00000000-0000-4000-8000-000000000000	\N	\N
1	870191ea-f8e4-40f3-bb6e-d8fcbc16e3f2	\N	\N
\.


--
-- TOC entry 6330 (class 0 OID 16936)
-- Dependencies: 292
-- Data for Name: vcs_root_first_revision; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_root_first_revision (build_type_id, parent_root_id, settings_hash, vcs_revision) FROM stdin;
\.


--
-- TOC entry 6322 (class 0 OID 16875)
-- Dependencies: 284
-- Data for Name: vcs_root_instance; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_root_instance (id, parent_id, settings_hash, body) FROM stdin;
1	1	-5618871651824901478	agentCleanFilesPolicy=ALL_UNTRACKED\nagentCleanPolicy=ON_BRANCH_CHANGE\nauthMethod=PASSWORD\nbranch=refs/heads/main\nignoreKnownHosts=true\nsecure:password=zxx6c762c0be3a3e992c3db07e945307465\nsubmoduleCheckout=CHECKOUT\nteamcity:branchSpec=refs/heads/*\nteamcity:vcsRootName=https://github.com/shwaindog/Fortitude#refs/heads/main\nurl=https://github.com/shwaindog/Fortitude\nuseAlternates=AUTO\nusername=shwaindog@gmail.com\nusernameStyle=USERID\nvcs=jetbrains.git\n
2	1	-5506108447151953391	agentCleanFilesPolicy=ALL_UNTRACKED\nagentCleanPolicy=ON_BRANCH_CHANGE\nauthMethod=TEAMCITY_SSH_KEY\nbranch=refs/heads/main\nignoreKnownHosts=true\nsecure:password=zxx775d03cbe80d301b\nsubmoduleCheckout=CHECKOUT\nteamcity:branchSpec=refs/heads/*\nteamcity:vcsRootName=ssh://git@github.com:shwaindog/Fortitude.git#refs/heads/main\nteamcitySshKey=AlexisGitHub\nurl=git@github.com:shwaindog/Fortitude.git\nuseAlternates=AUTO\nusername=git\nusernameStyle=USERID\nvcs=jetbrains.git\n
\.


--
-- TOC entry 6283 (class 0 OID 16616)
-- Dependencies: 245
-- Data for Name: vcs_root_mapping; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_root_mapping (int_id, ext_id, main) FROM stdin;
-1	_deleted_vcs_root	1
1	Fortitude_SshGitGithubComShwaindogFortitudeGitRefsHeadsMain	1
1	Fortitude_HttpsGithubComShwaindogFortitudeRefsHeadsMain	0
\.


--
-- TOC entry 6331 (class 0 OID 16941)
-- Dependencies: 293
-- Data for Name: vcs_username; Type: TABLE DATA; Schema: public; Owner: teamcity
--

COPY public.vcs_username (user_id, vcs_name, parent_vcs_root_id, order_num, username) FROM stdin;
1	anyVcs	-1	0	fortitude
\.


--
-- TOC entry 5765 (class 2606 OID 16671)
-- Name: agent agent_name_ui; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent
    ADD CONSTRAINT agent_name_ui UNIQUE (name);


--
-- TOC entry 5767 (class 2606 OID 16669)
-- Name: agent agent_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent
    ADD CONSTRAINT agent_pk PRIMARY KEY (id);


--
-- TOC entry 5747 (class 2606 OID 16632)
-- Name: agent_pool agent_pool_id_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent_pool
    ADD CONSTRAINT agent_pool_id_pk PRIMARY KEY (agent_pool_id);


--
-- TOC entry 5858 (class 2606 OID 16887)
-- Name: agent_pool_project agent_pool_project_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent_pool_project
    ADD CONSTRAINT agent_pool_project_pk PRIMARY KEY (agent_pool_id, project_int_id);


--
-- TOC entry 5749 (class 2606 OID 16639)
-- Name: agent_type agent_type_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent_type
    ADD CONSTRAINT agent_type_ak UNIQUE (cloud_code, profile_id, image_id);


--
-- TOC entry 5844 (class 2606 OID 16855)
-- Name: agent_type_bt_access agent_type_bt_access_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent_type_bt_access
    ADD CONSTRAINT agent_type_bt_access_pk PRIMARY KEY (agent_type_id, build_type_id);


--
-- TOC entry 5754 (class 2606 OID 16645)
-- Name: agent_type_info agent_type_info_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent_type_info
    ADD CONSTRAINT agent_type_info_pk PRIMARY KEY (agent_type_id);


--
-- TOC entry 5760 (class 2606 OID 16662)
-- Name: agent_type_param agent_type_param_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent_type_param
    ADD CONSTRAINT agent_type_param_pk PRIMARY KEY (agent_type_id, param_kind, param_name);


--
-- TOC entry 5751 (class 2606 OID 16637)
-- Name: agent_type agent_type_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent_type
    ADD CONSTRAINT agent_type_pk PRIMARY KEY (agent_type_id);


--
-- TOC entry 5756 (class 2606 OID 16650)
-- Name: agent_type_runner agent_type_runner_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent_type_runner
    ADD CONSTRAINT agent_type_runner_pk PRIMARY KEY (agent_type_id, runner);


--
-- TOC entry 5758 (class 2606 OID 16655)
-- Name: agent_type_vcs agent_type_vcs_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.agent_type_vcs
    ADD CONSTRAINT agent_type_vcs_pk PRIMARY KEY (agent_type_id, vcs);


--
-- TOC entry 5715 (class 2606 OID 16570)
-- Name: backup_builds backup_builds_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.backup_builds
    ADD CONSTRAINT backup_builds_pk PRIMARY KEY (build_id);


--
-- TOC entry 5713 (class 2606 OID 16564)
-- Name: backup_info backup_info_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.backup_info
    ADD CONSTRAINT backup_info_pk PRIMARY KEY (mproc_id);


--
-- TOC entry 5921 (class 2606 OID 17019)
-- Name: build_attrs build_attrs_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_attrs
    ADD CONSTRAINT build_attrs_pk PRIMARY KEY (build_state_id, attr_name);


--
-- TOC entry 5970 (class 2606 OID 17140)
-- Name: build_checkout_rules build_checkout_rules_vid_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_checkout_rules
    ADD CONSTRAINT build_checkout_rules_vid_pk PRIMARY KEY (build_state_id, vcs_root_id);


--
-- TOC entry 5923 (class 2606 OID 17026)
-- Name: build_data_storage build_data_storage_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_data_storage
    ADD CONSTRAINT build_data_storage_pk PRIMARY KEY (build_id, metric_id);


--
-- TOC entry 5917 (class 2606 OID 17011)
-- Name: build_dependency build_dependency_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_dependency
    ADD CONSTRAINT build_dependency_pk PRIMARY KEY (build_state_id, depends_on);


--
-- TOC entry 6023 (class 2606 OID 17280)
-- Name: build_labels build_labels_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_labels
    ADD CONSTRAINT build_labels_pk PRIMARY KEY (build_id, vcs_root_id);


--
-- TOC entry 6039 (class 2606 OID 17315)
-- Name: build_overriden_roots build_overriden_roots_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_overriden_roots
    ADD CONSTRAINT build_overriden_roots_pk PRIMARY KEY (build_state_id, original_vcs_root_id);


--
-- TOC entry 5952 (class 2606 OID 17091)
-- Name: build_problem_attribute build_problem_attribute_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_problem_attribute
    ADD CONSTRAINT build_problem_attribute_pk PRIMARY KEY (build_state_id, problem_id, attr_name);


--
-- TOC entry 5998 (class 2606 OID 17196)
-- Name: build_problem_muted build_problem_muted_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_problem_muted
    ADD CONSTRAINT build_problem_muted_pk PRIMARY KEY (build_state_id, problem_id);


--
-- TOC entry 5949 (class 2606 OID 17083)
-- Name: build_problem build_problem_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_problem
    ADD CONSTRAINT build_problem_pk PRIMARY KEY (build_state_id, problem_id);


--
-- TOC entry 5913 (class 2606 OID 17005)
-- Name: build_project build_project_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_project
    ADD CONSTRAINT build_project_pk PRIMARY KEY (build_id, project_level);


--
-- TOC entry 6007 (class 2606 OID 17222)
-- Name: build_queue_order build_queue_order_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_queue_order
    ADD CONSTRAINT build_queue_order_pk PRIMARY KEY (version, line_num);


--
-- TOC entry 6018 (class 2606 OID 17260)
-- Name: build_revisions build_revisions_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_revisions
    ADD CONSTRAINT build_revisions_pk PRIMARY KEY (build_state_id, vcs_root_id);


--
-- TOC entry 6087 (class 2606 OID 17427)
-- Name: build_set_tmp build_set_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_set_tmp
    ADD CONSTRAINT build_set_pk PRIMARY KEY (build_id);


--
-- TOC entry 5888 (class 2606 OID 16959)
-- Name: build_state build_state_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_state
    ADD CONSTRAINT build_state_pk PRIMARY KEY (id);


--
-- TOC entry 6036 (class 2606 OID 17309)
-- Name: build_state_private_tag build_state_private_tag_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_state_private_tag
    ADD CONSTRAINT build_state_private_tag_pk PRIMARY KEY (build_state_id, owner, tag);


--
-- TOC entry 6033 (class 2606 OID 17303)
-- Name: build_state_tag build_state_tag_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_state_tag
    ADD CONSTRAINT build_state_tag_pk PRIMARY KEY (build_state_id, tag);


--
-- TOC entry 5725 (class 2606 OID 16594)
-- Name: build_type build_type_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_type
    ADD CONSTRAINT build_type_ak UNIQUE (config_id);


--
-- TOC entry 5745 (class 2606 OID 16627)
-- Name: build_type_counters build_type_counters_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_type_counters
    ADD CONSTRAINT build_type_counters_pk PRIMARY KEY (build_type_id);


--
-- TOC entry 5961 (class 2606 OID 17116)
-- Name: build_type_edge_relation build_type_edge_relation_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_type_edge_relation
    ADD CONSTRAINT build_type_edge_relation_pk PRIMARY KEY (child_modification_id, build_type_id, parent_num);


--
-- TOC entry 5737 (class 2606 OID 16615)
-- Name: build_type_mapping build_type_mapping_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_type_mapping
    ADD CONSTRAINT build_type_mapping_ak UNIQUE (ext_id);


--
-- TOC entry 5739 (class 2606 OID 16613)
-- Name: build_type_mapping build_type_mapping_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_type_mapping
    ADD CONSTRAINT build_type_mapping_pk PRIMARY KEY (int_id, ext_id);


--
-- TOC entry 5727 (class 2606 OID 16592)
-- Name: build_type build_type_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_type
    ADD CONSTRAINT build_type_pk PRIMARY KEY (int_id);


--
-- TOC entry 5958 (class 2606 OID 17110)
-- Name: build_type_vcs_change build_type_vcs_change_ui; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.build_type_vcs_change
    ADD CONSTRAINT build_type_vcs_change_ui UNIQUE (modification_id, build_type_id);


--
-- TOC entry 5925 (class 2606 OID 17031)
-- Name: canceled_info canceled_info_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.canceled_info
    ADD CONSTRAINT canceled_info_pk PRIMARY KEY (build_id);


--
-- TOC entry 6089 (class 2606 OID 17432)
-- Name: clean_checkout_enforcement clean_checkout_enforcement_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.clean_checkout_enforcement
    ADD CONSTRAINT clean_checkout_enforcement_pk PRIMARY KEY (build_type_id, agent_id);


--
-- TOC entry 5717 (class 2606 OID 16575)
-- Name: cleanup_history cleanup_history_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.cleanup_history
    ADD CONSTRAINT cleanup_history_pk PRIMARY KEY (proc_id);


--
-- TOC entry 5769 (class 2606 OID 16679)
-- Name: cloud_image_state cloud_image_state_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.cloud_image_state
    ADD CONSTRAINT cloud_image_state_pk PRIMARY KEY (project_id, profile_id, image_id);


--
-- TOC entry 5777 (class 2606 OID 16701)
-- Name: cloud_image_without_agent cloud_image_without_agent_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.cloud_image_without_agent
    ADD CONSTRAINT cloud_image_without_agent_pk PRIMARY KEY (profile_id, cloud_code, image_id);


--
-- TOC entry 5771 (class 2606 OID 16684)
-- Name: cloud_instance_state cloud_instance_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.cloud_instance_state
    ADD CONSTRAINT cloud_instance_pk PRIMARY KEY (project_id, profile_id, image_id, instance_id);


--
-- TOC entry 5775 (class 2606 OID 16696)
-- Name: cloud_started_instance cloud_started_instance_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.cloud_started_instance
    ADD CONSTRAINT cloud_started_instance_pk PRIMARY KEY (profile_id, cloud_code, image_id, instance_id);


--
-- TOC entry 5773 (class 2606 OID 16691)
-- Name: cloud_state_data cloud_state_data_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.cloud_state_data
    ADD CONSTRAINT cloud_state_data_pk PRIMARY KEY (project_id, profile_id, image_id, instance_id);


--
-- TOC entry 6080 (class 2606 OID 17409)
-- Name: comments comments_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.comments
    ADD CONSTRAINT comments_pk PRIMARY KEY (id);


--
-- TOC entry 6117 (class 2606 OID 17500)
-- Name: config_persisting_tasks config_persisting_tasks_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.config_persisting_tasks
    ADD CONSTRAINT config_persisting_tasks_pk PRIMARY KEY (id, task_type);


--
-- TOC entry 6105 (class 2606 OID 17472)
-- Name: custom_data_body custom_data_body_idx; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.custom_data_body
    ADD CONSTRAINT custom_data_body_idx UNIQUE (id, part_num);


--
-- TOC entry 6110 (class 2606 OID 17480)
-- Name: custom_data custom_data_key_hash_idx; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.custom_data
    ADD CONSTRAINT custom_data_key_hash_idx UNIQUE (data_key_hash, collision_idx);


--
-- TOC entry 5710 (class 2606 OID 16557)
-- Name: db_heartbeat db_heartbeat_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.db_heartbeat
    ADD CONSTRAINT db_heartbeat_pk PRIMARY KEY (starting_code);


--
-- TOC entry 5704 (class 2606 OID 16537)
-- Name: db_version db_version_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.db_version
    ADD CONSTRAINT db_version_pk PRIMARY KEY (version_number);


--
-- TOC entry 5719 (class 2606 OID 16580)
-- Name: domain_sequence domain_name_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.domain_sequence
    ADD CONSTRAINT domain_name_pk PRIMARY KEY (domain_name);


--
-- TOC entry 6071 (class 2606 OID 17386)
-- Name: duplicate_diff duplicate_diff_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.duplicate_diff
    ADD CONSTRAINT duplicate_diff_pk PRIMARY KEY (build_id, hash);


--
-- TOC entry 6074 (class 2606 OID 17391)
-- Name: duplicate_fragments duplicate_fragments_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.duplicate_fragments
    ADD CONSTRAINT duplicate_fragments_pk PRIMARY KEY (id, file_id, line, offset_info);


--
-- TOC entry 6069 (class 2606 OID 17380)
-- Name: duplicate_results duplicate_results_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.duplicate_results
    ADD CONSTRAINT duplicate_results_pk PRIMARY KEY (id);


--
-- TOC entry 6076 (class 2606 OID 17397)
-- Name: duplicate_stats duplicate_stats_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.duplicate_stats
    ADD CONSTRAINT duplicate_stats_pk PRIMARY KEY (build_id);


--
-- TOC entry 6011 (class 2606 OID 17234)
-- Name: failed_tests_output failed_tests_output_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.failed_tests_output
    ADD CONSTRAINT failed_tests_output_pk PRIMARY KEY (build_id, test_id);


--
-- TOC entry 5929 (class 2606 OID 17036)
-- Name: failed_tests failed_tests_pk2; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.failed_tests
    ADD CONSTRAINT failed_tests_pk2 PRIMARY KEY (test_name_id, build_id);


--
-- TOC entry 5815 (class 2606 OID 16797)
-- Name: usergroup_notification_data group_notif_data_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.usergroup_notification_data
    ADD CONSTRAINT group_notif_data_pk PRIMARY KEY (group_id, rule_id);


--
-- TOC entry 5899 (class 2606 OID 16980)
-- Name: history history_build_id_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.history
    ADD CONSTRAINT history_build_id_pk PRIMARY KEY (build_id);


--
-- TOC entry 5964 (class 2606 OID 17122)
-- Name: ids_group ids_group_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.ids_group
    ADD CONSTRAINT ids_group_pk PRIMARY KEY (id);


--
-- TOC entry 6054 (class 2606 OID 17346)
-- Name: inspection_data inspection_data_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.inspection_data
    ADD CONSTRAINT inspection_data_pk PRIMARY KEY (hash);


--
-- TOC entry 6061 (class 2606 OID 17367)
-- Name: inspection_diff inspection_diff_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.inspection_diff
    ADD CONSTRAINT inspection_diff_ak UNIQUE (build_id, hash);


--
-- TOC entry 6048 (class 2606 OID 17338)
-- Name: inspection_info inspection_info_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.inspection_info
    ADD CONSTRAINT inspection_info_ak UNIQUE (inspection_id);


--
-- TOC entry 6050 (class 2606 OID 17336)
-- Name: inspection_info inspection_info_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.inspection_info
    ADD CONSTRAINT inspection_info_pk PRIMARY KEY (id);


--
-- TOC entry 6059 (class 2606 OID 17362)
-- Name: inspection_stats inspection_stats_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.inspection_stats
    ADD CONSTRAINT inspection_stats_pk PRIMARY KEY (build_id);


--
-- TOC entry 6123 (class 2606 OID 17520)
-- Name: light_history light_history_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.light_history
    ADD CONSTRAINT light_history_pk PRIMARY KEY (build_id);


--
-- TOC entry 5830 (class 2606 OID 16828)
-- Name: long_file_name long_file_name_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.long_file_name
    ADD CONSTRAINT long_file_name_pk PRIMARY KEY (hash);


--
-- TOC entry 5706 (class 2606 OID 16542)
-- Name: meta_file_line meta_file_line_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.meta_file_line
    ADD CONSTRAINT meta_file_line_pk PRIMARY KEY (file_name, line_nr);


--
-- TOC entry 5835 (class 2606 OID 16841)
-- Name: data_storage_dict metric_id_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.data_storage_dict
    ADD CONSTRAINT metric_id_pk PRIMARY KEY (metric_id);


--
-- TOC entry 5972 (class 2606 OID 17149)
-- Name: mute_info mute_info_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.mute_info
    ADD CONSTRAINT mute_info_ak UNIQUE (project_int_id, mute_id);


--
-- TOC entry 5977 (class 2606 OID 17154)
-- Name: mute_info_bt mute_info_bt_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.mute_info_bt
    ADD CONSTRAINT mute_info_bt_pk PRIMARY KEY (mute_id, build_type_id);


--
-- TOC entry 5974 (class 2606 OID 17147)
-- Name: mute_info mute_info_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.mute_info
    ADD CONSTRAINT mute_info_pk PRIMARY KEY (mute_id);


--
-- TOC entry 5989 (class 2606 OID 17179)
-- Name: mute_info_problem mute_info_problem_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.mute_info_problem
    ADD CONSTRAINT mute_info_problem_pk PRIMARY KEY (mute_id, problem_id);


--
-- TOC entry 5979 (class 2606 OID 17160)
-- Name: mute_info_test mute_info_test_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.mute_info_test
    ADD CONSTRAINT mute_info_test_pk PRIMARY KEY (mute_id, test_name_id);


--
-- TOC entry 5995 (class 2606 OID 17190)
-- Name: mute_problem_in_bt mute_problem_in_bt_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.mute_problem_in_bt
    ADD CONSTRAINT mute_problem_in_bt_pk PRIMARY KEY (mute_id, build_type_id, problem_id);


--
-- TOC entry 5992 (class 2606 OID 17184)
-- Name: mute_problem_in_proj mute_problem_in_proj_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.mute_problem_in_proj
    ADD CONSTRAINT mute_problem_in_proj_pk PRIMARY KEY (mute_id, project_int_id, problem_id);


--
-- TOC entry 5986 (class 2606 OID 17172)
-- Name: mute_test_in_bt mute_test_in_bt_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.mute_test_in_bt
    ADD CONSTRAINT mute_test_in_bt_pk PRIMARY KEY (mute_id, build_type_id, test_name_id);


--
-- TOC entry 5982 (class 2606 OID 17165)
-- Name: mute_test_in_proj mute_test_in_proj_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.mute_test_in_proj
    ADD CONSTRAINT mute_test_in_proj_pk PRIMARY KEY (mute_id, project_int_id, test_name_id);


--
-- TOC entry 6093 (class 2606 OID 17441)
-- Name: node_events node_events_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.node_events
    ADD CONSTRAINT node_events_pk PRIMARY KEY (node_id, id);


--
-- TOC entry 6103 (class 2606 OID 17464)
-- Name: node_locks node_locks_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.node_locks
    ADD CONSTRAINT node_locks_pk PRIMARY KEY (lock_type, id);


--
-- TOC entry 6100 (class 2606 OID 17459)
-- Name: node_tasks_long_value node_tasks_long_value_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.node_tasks_long_value
    ADD CONSTRAINT node_tasks_long_value_pk PRIMARY KEY (uuid);


--
-- TOC entry 6095 (class 2606 OID 17449)
-- Name: node_tasks node_tasks_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.node_tasks
    ADD CONSTRAINT node_tasks_pk PRIMARY KEY (id);


--
-- TOC entry 6098 (class 2606 OID 17451)
-- Name: node_tasks node_tasks_type_ident_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.node_tasks
    ADD CONSTRAINT node_tasks_type_ident_ak UNIQUE (task_type, task_identity, task_state);


--
-- TOC entry 5821 (class 2606 OID 16810)
-- Name: permanent_tokens permanent_token_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.permanent_tokens
    ADD CONSTRAINT permanent_token_pk PRIMARY KEY (id);


--
-- TOC entry 6026 (class 2606 OID 17288)
-- Name: personal_build_relative_path personal_build_relative_p_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.personal_build_relative_path
    ADD CONSTRAINT personal_build_relative_p_ak UNIQUE (build_id, original_path_hash);


--
-- TOC entry 5871 (class 2606 OID 16922)
-- Name: personal_vcs_change personal_vcs_changes_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.personal_vcs_change
    ADD CONSTRAINT personal_vcs_changes_pk PRIMARY KEY (modification_id, file_num);


--
-- TOC entry 5866 (class 2606 OID 16914)
-- Name: personal_vcs_history personal_vcs_history_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.personal_vcs_history
    ADD CONSTRAINT personal_vcs_history_ak UNIQUE (modification_hash);


--
-- TOC entry 5868 (class 2606 OID 16912)
-- Name: personal_vcs_history personal_vcs_history_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.personal_vcs_history
    ADD CONSTRAINT personal_vcs_history_pk PRIMARY KEY (modification_id);


--
-- TOC entry 5839 (class 2606 OID 16850)
-- Name: problem problem_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.problem
    ADD CONSTRAINT problem_ak UNIQUE (problem_type, problem_identity);


--
-- TOC entry 5841 (class 2606 OID 16848)
-- Name: problem problem_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.problem
    ADD CONSTRAINT problem_pk PRIMARY KEY (problem_id);


--
-- TOC entry 5721 (class 2606 OID 16587)
-- Name: project project_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.project
    ADD CONSTRAINT project_ak UNIQUE (config_id);


--
-- TOC entry 6064 (class 2606 OID 17375)
-- Name: project_files project_files_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.project_files
    ADD CONSTRAINT project_files_ak UNIQUE (file_name);


--
-- TOC entry 6066 (class 2606 OID 17373)
-- Name: project_files project_files_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.project_files
    ADD CONSTRAINT project_files_pk PRIMARY KEY (file_id);


--
-- TOC entry 5733 (class 2606 OID 16608)
-- Name: project_mapping project_mapping_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.project_mapping
    ADD CONSTRAINT project_mapping_ak UNIQUE (ext_id);


--
-- TOC entry 5735 (class 2606 OID 16606)
-- Name: project_mapping project_mapping_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.project_mapping
    ADD CONSTRAINT project_mapping_pk PRIMARY KEY (int_id, ext_id);


--
-- TOC entry 5723 (class 2606 OID 16585)
-- Name: project project_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.project
    ADD CONSTRAINT project_pk PRIMARY KEY (int_id);


--
-- TOC entry 5911 (class 2606 OID 16996)
-- Name: removed_builds_history removed_builds_history_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.removed_builds_history
    ADD CONSTRAINT removed_builds_history_pk PRIMARY KEY (build_id);


--
-- TOC entry 6029 (class 2606 OID 17296)
-- Name: responsibilities responsibilities_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.responsibilities
    ADD CONSTRAINT responsibilities_pk PRIMARY KEY (problem_id);


--
-- TOC entry 5893 (class 2606 OID 16972)
-- Name: running running_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.running
    ADD CONSTRAINT running_pk PRIMARY KEY (build_id);


--
-- TOC entry 6113 (class 2606 OID 17487)
-- Name: server_health_items server_health_items_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.server_health_items
    ADD CONSTRAINT server_health_items_pk PRIMARY KEY (id);


--
-- TOC entry 5708 (class 2606 OID 16550)
-- Name: server_property server_property_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.server_property
    ADD CONSTRAINT server_property_pk PRIMARY KEY (prop_name);


--
-- TOC entry 6009 (class 2606 OID 17227)
-- Name: stats stats_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.stats
    ADD CONSTRAINT stats_pk PRIMARY KEY (build_id);


--
-- TOC entry 6078 (class 2606 OID 17402)
-- Name: stats_publisher_state stats_publisher_state_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.stats_publisher_state
    ADD CONSTRAINT stats_publisher_state_pk PRIMARY KEY (metric_id);


--
-- TOC entry 6003 (class 2606 OID 17208)
-- Name: test_failure_rate test_failure_rate_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_failure_rate
    ADD CONSTRAINT test_failure_rate_pk PRIMARY KEY (build_type_id, test_name_id);


--
-- TOC entry 5935 (class 2606 OID 17051)
-- Name: test_info_archive test_info_archive_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_info_archive
    ADD CONSTRAINT test_info_archive_pk PRIMARY KEY (build_id, test_id);


--
-- TOC entry 5931 (class 2606 OID 17044)
-- Name: test_info test_info_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_info
    ADD CONSTRAINT test_info_pk PRIMARY KEY (build_id, test_id);


--
-- TOC entry 5937 (class 2606 OID 17061)
-- Name: test_metadata_dict test_metadata_dict_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_metadata_dict
    ADD CONSTRAINT test_metadata_dict_ak UNIQUE (name_digest);


--
-- TOC entry 5939 (class 2606 OID 17059)
-- Name: test_metadata_dict test_metadata_dict_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_metadata_dict
    ADD CONSTRAINT test_metadata_dict_pk PRIMARY KEY (key_id);


--
-- TOC entry 5945 (class 2606 OID 17075)
-- Name: test_metadata test_metadata_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_metadata
    ADD CONSTRAINT test_metadata_pk PRIMARY KEY (build_id, test_id, key_id);


--
-- TOC entry 5941 (class 2606 OID 17068)
-- Name: test_metadata_types test_metadata_types_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_metadata_types
    ADD CONSTRAINT test_metadata_types_ak UNIQUE (name);


--
-- TOC entry 5943 (class 2606 OID 17066)
-- Name: test_metadata_types test_metadata_types_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_metadata_types
    ADD CONSTRAINT test_metadata_types_pk PRIMARY KEY (type_id);


--
-- TOC entry 6001 (class 2606 OID 17202)
-- Name: test_muted test_muted_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_muted
    ADD CONSTRAINT test_muted_pk PRIMARY KEY (build_id, test_name_id, mute_id);


--
-- TOC entry 5833 (class 2606 OID 16835)
-- Name: test_names test_names_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.test_names
    ADD CONSTRAINT test_names_pk PRIMARY KEY (id);


--
-- TOC entry 5823 (class 2606 OID 16814)
-- Name: permanent_tokens token_identifier_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.permanent_tokens
    ADD CONSTRAINT token_identifier_ak UNIQUE (identifier);


--
-- TOC entry 5828 (class 2606 OID 16820)
-- Name: permanent_token_permissions token_permissions_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.permanent_token_permissions
    ADD CONSTRAINT token_permissions_pk PRIMARY KEY (id, project_id, permission);


--
-- TOC entry 5825 (class 2606 OID 16812)
-- Name: permanent_tokens token_user_id_name_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.permanent_tokens
    ADD CONSTRAINT token_user_id_name_ak UNIQUE (user_id, name);


--
-- TOC entry 5792 (class 2606 OID 16741)
-- Name: user_attribute user_attr_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.user_attribute
    ADD CONSTRAINT user_attr_pk PRIMARY KEY (user_id, attr_key);


--
-- TOC entry 5794 (class 2606 OID 16748)
-- Name: user_blocks user_blocks_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.user_blocks
    ADD CONSTRAINT user_blocks_pk PRIMARY KEY (user_id, block_type);


--
-- TOC entry 5852 (class 2606 OID 16873)
-- Name: user_build_types_order user_bt_order_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.user_build_types_order
    ADD CONSTRAINT user_bt_order_pk PRIMARY KEY (user_id, project_int_id, bt_int_id);


--
-- TOC entry 5802 (class 2606 OID 16767)
-- Name: user_notification_data user_notif_data_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.user_notification_data
    ADD CONSTRAINT user_notif_data_pk PRIMARY KEY (user_id, rule_id);


--
-- TOC entry 5798 (class 2606 OID 16753)
-- Name: user_notification_events user_notification_events_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.user_notification_events
    ADD CONSTRAINT user_notification_events_pk PRIMARY KEY (id);


--
-- TOC entry 5850 (class 2606 OID 16867)
-- Name: user_projects_order user_projects_order_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.user_projects_order
    ADD CONSTRAINT user_projects_order_pk PRIMARY KEY (user_id, project_int_id);


--
-- TOC entry 5847 (class 2606 OID 16861)
-- Name: user_projects_visibility user_projects_visibility_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.user_projects_visibility
    ADD CONSTRAINT user_projects_visibility_pk PRIMARY KEY (user_id, project_int_id);


--
-- TOC entry 5790 (class 2606 OID 16733)
-- Name: user_property user_property_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.user_property
    ADD CONSTRAINT user_property_pk PRIMARY KEY (user_id, prop_key);


--
-- TOC entry 6043 (class 2606 OID 17322)
-- Name: user_roles user_roles_ui; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT user_roles_ui UNIQUE (user_id, role_id, project_int_id);


--
-- TOC entry 5811 (class 2606 OID 16783)
-- Name: usergroup_notification_events usergroup_notific_evnts_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.usergroup_notification_events
    ADD CONSTRAINT usergroup_notific_evnts_pk PRIMARY KEY (id);


--
-- TOC entry 5783 (class 2606 OID 16717)
-- Name: usergroup_property usergroup_property_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.usergroup_property
    ADD CONSTRAINT usergroup_property_pk PRIMARY KEY (group_id, prop_key);


--
-- TOC entry 6046 (class 2606 OID 17328)
-- Name: usergroup_roles usergroup_roles_ui; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.usergroup_roles
    ADD CONSTRAINT usergroup_roles_ui UNIQUE (group_id, role_id, project_int_id);


--
-- TOC entry 5805 (class 2606 OID 16773)
-- Name: usergroup_subgroups usergroup_subgroups_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.usergroup_subgroups
    ADD CONSTRAINT usergroup_subgroups_pk PRIMARY KEY (hostgroup_id, subgroup_id);


--
-- TOC entry 5807 (class 2606 OID 16778)
-- Name: usergroup_users usergroup_users_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.usergroup_users
    ADD CONSTRAINT usergroup_users_pk PRIMARY KEY (group_id, user_id);


--
-- TOC entry 5779 (class 2606 OID 16710)
-- Name: usergroups usergroups_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.usergroups
    ADD CONSTRAINT usergroups_ak UNIQUE (name);


--
-- TOC entry 5781 (class 2606 OID 16708)
-- Name: usergroups usergroups_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.usergroups
    ADD CONSTRAINT usergroups_pk PRIMARY KEY (group_id);


--
-- TOC entry 5785 (class 2606 OID 16726)
-- Name: users users_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_ak UNIQUE (username);


--
-- TOC entry 5787 (class 2606 OID 16724)
-- Name: users users_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pk PRIMARY KEY (id);


--
-- TOC entry 5837 (class 2606 OID 16843)
-- Name: data_storage_dict value_type_key_index; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.data_storage_dict
    ADD CONSTRAINT value_type_key_index UNIQUE (value_type_key);


--
-- TOC entry 5876 (class 2606 OID 16935)
-- Name: vcs_change_attrs vcs_change_attrs_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_change_attrs
    ADD CONSTRAINT vcs_change_attrs_pk PRIMARY KEY (modification_id, attr_name);


--
-- TOC entry 5864 (class 2606 OID 16903)
-- Name: vcs_change vcs_change_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_change
    ADD CONSTRAINT vcs_change_pk PRIMARY KEY (modification_id, file_num);


--
-- TOC entry 5874 (class 2606 OID 16927)
-- Name: vcs_changes_graph vcs_changes_graph_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_changes_graph
    ADD CONSTRAINT vcs_changes_graph_pk PRIMARY KEY (child_modification_id, parent_num);


--
-- TOC entry 5861 (class 2606 OID 16894)
-- Name: vcs_history vcs_history_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_history
    ADD CONSTRAINT vcs_history_pk PRIMARY KEY (modification_id);


--
-- TOC entry 5729 (class 2606 OID 16601)
-- Name: vcs_root vcs_root_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_root
    ADD CONSTRAINT vcs_root_ak UNIQUE (config_id);


--
-- TOC entry 5878 (class 2606 OID 16940)
-- Name: vcs_root_first_revision vcs_root_first_revision_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_root_first_revision
    ADD CONSTRAINT vcs_root_first_revision_pk PRIMARY KEY (build_type_id, parent_root_id, settings_hash);


--
-- TOC entry 5856 (class 2606 OID 16881)
-- Name: vcs_root_instance vcs_root_instance_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_root_instance
    ADD CONSTRAINT vcs_root_instance_pk PRIMARY KEY (id);


--
-- TOC entry 5741 (class 2606 OID 16622)
-- Name: vcs_root_mapping vcs_root_mapping_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_root_mapping
    ADD CONSTRAINT vcs_root_mapping_ak UNIQUE (ext_id);


--
-- TOC entry 5743 (class 2606 OID 16620)
-- Name: vcs_root_mapping vcs_root_mapping_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_root_mapping
    ADD CONSTRAINT vcs_root_mapping_pk PRIMARY KEY (int_id, ext_id);


--
-- TOC entry 5731 (class 2606 OID 16599)
-- Name: vcs_root vcs_root_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_root
    ADD CONSTRAINT vcs_root_pk PRIMARY KEY (int_id);


--
-- TOC entry 5880 (class 2606 OID 16947)
-- Name: vcs_username vcs_username_ak; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_username
    ADD CONSTRAINT vcs_username_ak UNIQUE (user_id, vcs_name, parent_vcs_root_id, username);


--
-- TOC entry 5882 (class 2606 OID 16945)
-- Name: vcs_username vcs_username_pk; Type: CONSTRAINT; Schema: public; Owner: teamcity
--

ALTER TABLE ONLY public.vcs_username
    ADD CONSTRAINT vcs_username_pk PRIMARY KEY (user_id, vcs_name, parent_vcs_root_id, order_num);


--
-- TOC entry 6082 (class 1259 OID 17416)
-- Name: action_history_action_object_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX action_history_action_object_i ON public.action_history USING btree (action, object_id);


--
-- TOC entry 6083 (class 1259 OID 17414)
-- Name: action_history_comment; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX action_history_comment ON public.action_history USING btree (comment_id);


--
-- TOC entry 6084 (class 1259 OID 17415)
-- Name: action_history_object; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX action_history_object ON public.action_history USING btree (object_id);


--
-- TOC entry 5761 (class 1259 OID 16674)
-- Name: agent_agent_type_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX agent_agent_type_id ON public.agent USING btree (agent_type_id);


--
-- TOC entry 5762 (class 1259 OID 16673)
-- Name: agent_authorization_token; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX agent_authorization_token ON public.agent USING btree (authorization_token);


--
-- TOC entry 5763 (class 1259 OID 16672)
-- Name: agent_host_address; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX agent_host_address ON public.agent USING btree (host_addr);


--
-- TOC entry 5842 (class 1259 OID 16856)
-- Name: agent_type_bt_access_bt_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX agent_type_bt_access_bt_i ON public.agent_type_bt_access USING btree (build_type_id);


--
-- TOC entry 5752 (class 1259 OID 16640)
-- Name: agent_type_pool_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX agent_type_pool_i ON public.agent_type USING btree (agent_pool_id);


--
-- TOC entry 6085 (class 1259 OID 17422)
-- Name: audit_a_o_comment; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX audit_a_o_comment ON public.audit_additional_object USING btree (comment_id);


--
-- TOC entry 5711 (class 1259 OID 16565)
-- Name: backup_info_file_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX backup_info_file_i ON public.backup_info USING btree (file_name);


--
-- TOC entry 5959 (class 1259 OID 17117)
-- Name: bt_edge_relation_btid; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX bt_edge_relation_btid ON public.build_type_edge_relation USING btree (build_type_id);


--
-- TOC entry 5967 (class 1259 OID 17133)
-- Name: bt_grp_vcs_change_grp_mod_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX bt_grp_vcs_change_grp_mod_idx ON public.build_type_group_vcs_change USING btree (group_id, modification_id);


--
-- TOC entry 5968 (class 1259 OID 17132)
-- Name: bt_grp_vcs_change_mod_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX bt_grp_vcs_change_mod_idx ON public.build_type_group_vcs_change USING btree (modification_id);


--
-- TOC entry 5953 (class 1259 OID 17098)
-- Name: build_artif_dep_state_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_artif_dep_state_id ON public.build_artifact_dependency USING btree (build_state_id);


--
-- TOC entry 5918 (class 1259 OID 17021)
-- Name: build_attrs_name_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_attrs_name_idx ON public.build_attrs USING btree (attr_name);


--
-- TOC entry 5919 (class 1259 OID 17020)
-- Name: build_attrs_num_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_attrs_num_i ON public.build_attrs USING btree (attr_num_value, attr_name, build_state_id);


--
-- TOC entry 5915 (class 1259 OID 17012)
-- Name: build_dependency_ak; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_dependency_ak ON public.build_dependency USING btree (depends_on, build_state_id);


--
-- TOC entry 6024 (class 1259 OID 17281)
-- Name: build_labels_vcs_root_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_labels_vcs_root_i ON public.build_labels USING btree (vcs_root_id);


--
-- TOC entry 6037 (class 1259 OID 17317)
-- Name: build_orig_root_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_orig_root_index ON public.build_overriden_roots USING btree (original_vcs_root_id);


--
-- TOC entry 5950 (class 1259 OID 17092)
-- Name: build_problem_attr_p_id_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_problem_attr_p_id_idx ON public.build_problem_attribute USING btree (problem_id);


--
-- TOC entry 5947 (class 1259 OID 17084)
-- Name: build_problem_id_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_problem_id_idx ON public.build_problem USING btree (problem_id);


--
-- TOC entry 5996 (class 1259 OID 17197)
-- Name: build_problem_mute_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_problem_mute_id ON public.build_problem_muted USING btree (mute_id);


--
-- TOC entry 5914 (class 1259 OID 17006)
-- Name: build_project_project_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_project_project_idx ON public.build_project USING btree (project_int_id);


--
-- TOC entry 6005 (class 1259 OID 17215)
-- Name: build_queue_build_state_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_queue_build_state_id ON public.build_queue USING btree (build_state_id);


--
-- TOC entry 6016 (class 1259 OID 17262)
-- Name: build_revisions_mod_id_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_revisions_mod_id_i ON public.build_revisions USING btree (modification_id);


--
-- TOC entry 6019 (class 1259 OID 17261)
-- Name: build_revisions_vcs_root_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_revisions_vcs_root_i ON public.build_revisions USING btree (vcs_root_id);


--
-- TOC entry 5884 (class 1259 OID 16960)
-- Name: build_state_build_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_state_build_i ON public.build_state USING btree (build_id, is_deleted, branch_name, is_personal);


--
-- TOC entry 5885 (class 1259 OID 16961)
-- Name: build_state_build_type_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_state_build_type_i ON public.build_state USING btree (build_type_id, is_deleted, branch_name, is_personal);


--
-- TOC entry 5886 (class 1259 OID 16962)
-- Name: build_state_mod_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_state_mod_i ON public.build_state USING btree (modification_id);


--
-- TOC entry 5889 (class 1259 OID 16964)
-- Name: build_state_pmod_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_state_pmod_i ON public.build_state USING btree (personal_modification_id);


--
-- TOC entry 6034 (class 1259 OID 17310)
-- Name: build_state_private_tag_ie1; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_state_private_tag_ie1 ON public.build_state_private_tag USING btree (owner, build_state_id);


--
-- TOC entry 5890 (class 1259 OID 16963)
-- Name: build_state_puser_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_state_puser_i ON public.build_state USING btree (personal_user_id);


--
-- TOC entry 5891 (class 1259 OID 16965)
-- Name: build_state_rem_queue_time_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_state_rem_queue_time_i ON public.build_state USING btree (remove_from_queue_time);


--
-- TOC entry 6031 (class 1259 OID 17304)
-- Name: build_state_tag_ie1; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_state_tag_ie1 ON public.build_state_tag USING btree (tag, build_state_id);


--
-- TOC entry 6040 (class 1259 OID 17316)
-- Name: build_subst_root_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_subst_root_index ON public.build_overriden_roots USING btree (substitution_vcs_root_id);


--
-- TOC entry 5956 (class 1259 OID 17111)
-- Name: build_type_vcs_change_btid; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX build_type_vcs_change_btid ON public.build_type_vcs_change USING btree (build_type_id);


--
-- TOC entry 6012 (class 1259 OID 17240)
-- Name: co_build_id_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX co_build_id_index ON public.compiler_output USING btree (build_id);


--
-- TOC entry 6081 (class 1259 OID 17410)
-- Name: comments_when_changed_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX comments_when_changed_i ON public.comments USING btree (when_changed);


--
-- TOC entry 6115 (class 1259 OID 17501)
-- Name: config_persisting_tasks_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX config_persisting_tasks_ie ON public.config_persisting_tasks USING btree (task_type, stage);


--
-- TOC entry 6107 (class 1259 OID 17482)
-- Name: custom_data_body_id_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX custom_data_body_id_idx ON public.custom_data USING btree (data_id);


--
-- TOC entry 6106 (class 1259 OID 17473)
-- Name: custom_data_body_ud_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX custom_data_body_ud_idx ON public.custom_data_body USING btree (update_date);


--
-- TOC entry 6108 (class 1259 OID 17481)
-- Name: custom_data_domain_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX custom_data_domain_idx ON public.custom_data USING btree (data_domain);


--
-- TOC entry 6020 (class 1259 OID 17268)
-- Name: def_build_params_state_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX def_build_params_state_id ON public.default_build_parameters USING btree (build_state_id);


--
-- TOC entry 6014 (class 1259 OID 17252)
-- Name: downloaded_artifacts_source_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX downloaded_artifacts_source_id ON public.downloaded_artifacts USING btree (source_build_id);


--
-- TOC entry 6015 (class 1259 OID 17253)
-- Name: downloaded_artifacts_ts_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX downloaded_artifacts_ts_id ON public.downloaded_artifacts USING btree (target_build_id, source_build_id);


--
-- TOC entry 6072 (class 1259 OID 17392)
-- Name: duplicate_fragments_file_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX duplicate_fragments_file_i ON public.duplicate_fragments USING btree (file_id);


--
-- TOC entry 6067 (class 1259 OID 17381)
-- Name: duplicate_results_build_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX duplicate_results_build_i ON public.duplicate_results USING btree (build_id);


--
-- TOC entry 5926 (class 1259 OID 17037)
-- Name: failed_tests_build_idx2; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX failed_tests_build_idx2 ON public.failed_tests USING btree (build_id);


--
-- TOC entry 5927 (class 1259 OID 17038)
-- Name: failed_tests_ffi_build_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX failed_tests_ffi_build_idx ON public.failed_tests USING btree (ffi_build_id);


--
-- TOC entry 5954 (class 1259 OID 17105)
-- Name: final_artif_dep_src_bt_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX final_artif_dep_src_bt_id ON public.final_artifact_dependency USING btree (source_build_type_id);


--
-- TOC entry 5955 (class 1259 OID 17104)
-- Name: final_artif_dep_state_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX final_artif_dep_state_id ON public.final_artifact_dependency USING btree (build_state_id);


--
-- TOC entry 5816 (class 1259 OID 16798)
-- Name: group_notif_data_rule_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX group_notif_data_rule_id ON public.usergroup_notification_data USING btree (rule_id);


--
-- TOC entry 5812 (class 1259 OID 16790)
-- Name: group_watch_type_rule_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX group_watch_type_rule_id ON public.usergroup_watch_type USING btree (rule_id);


--
-- TOC entry 6114 (class 1259 OID 17492)
-- Name: health_item_id_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX health_item_id_ie ON public.hidden_health_item USING btree (item_id);


--
-- TOC entry 5895 (class 1259 OID 16987)
-- Name: history_agent_finish_time_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX history_agent_finish_time_i ON public.history USING btree (agent_name, build_finish_time_server);


--
-- TOC entry 5896 (class 1259 OID 16989)
-- Name: history_agent_type_b_id_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX history_agent_type_b_id_i ON public.history USING btree (agent_type_id, build_id);


--
-- TOC entry 5897 (class 1259 OID 16983)
-- Name: history_bt_id_rm_from_q_time; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX history_bt_id_rm_from_q_time ON public.history USING btree (build_type_id, remove_from_queue_time);


--
-- TOC entry 5900 (class 1259 OID 16988)
-- Name: history_build_number_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX history_build_number_i ON public.history USING btree (build_number);


--
-- TOC entry 5901 (class 1259 OID 16986)
-- Name: history_build_state_id_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX history_build_state_id_i ON public.history USING btree (build_state_id);


--
-- TOC entry 5902 (class 1259 OID 16985)
-- Name: history_build_type_id_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX history_build_type_id_i ON public.history USING btree (build_type_id, branch_name, is_canceled, pin);


--
-- TOC entry 5903 (class 1259 OID 16982)
-- Name: history_finish_time_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX history_finish_time_idx ON public.history USING btree (build_finish_time_server);


--
-- TOC entry 5904 (class 1259 OID 16984)
-- Name: history_remove_from_q_time_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX history_remove_from_q_time_i ON public.history USING btree (remove_from_queue_time);


--
-- TOC entry 5905 (class 1259 OID 16981)
-- Name: history_start_time_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX history_start_time_idx ON public.history USING btree (build_start_time_server);


--
-- TOC entry 5965 (class 1259 OID 17128)
-- Name: ids_group_entity_id_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX ids_group_entity_id_idx ON public.ids_group_entity_id USING btree (entity_id);


--
-- TOC entry 5962 (class 1259 OID 17123)
-- Name: ids_group_hash_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX ids_group_hash_idx ON public.ids_group USING btree (group_hash);


--
-- TOC entry 5966 (class 1259 OID 17127)
-- Name: ids_group_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX ids_group_idx ON public.ids_group_entity_id USING btree (group_id, entity_id);


--
-- TOC entry 6013 (class 1259 OID 17246)
-- Name: ignored_tests_build_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX ignored_tests_build_id ON public.ignored_tests USING btree (build_id);


--
-- TOC entry 6051 (class 1259 OID 17347)
-- Name: inspection_data_file_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX inspection_data_file_index ON public.inspection_data USING btree (file_name);


--
-- TOC entry 6052 (class 1259 OID 17348)
-- Name: inspection_data_insp_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX inspection_data_insp_index ON public.inspection_data USING btree (inspection_id);


--
-- TOC entry 6062 (class 1259 OID 17368)
-- Name: inspection_diff_hash_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX inspection_diff_hash_index ON public.inspection_diff USING btree (hash);


--
-- TOC entry 6055 (class 1259 OID 17352)
-- Name: inspection_fixes_hash_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX inspection_fixes_hash_index ON public.inspection_fixes USING btree (hash);


--
-- TOC entry 6056 (class 1259 OID 17357)
-- Name: inspection_results_buildhash_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX inspection_results_buildhash_i ON public.inspection_results USING btree (build_id, hash);


--
-- TOC entry 6057 (class 1259 OID 17356)
-- Name: inspection_results_hash_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX inspection_results_hash_index ON public.inspection_results USING btree (hash);


--
-- TOC entry 6120 (class 1259 OID 17524)
-- Name: light_history_agt_b_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX light_history_agt_b_i ON public.light_history USING btree (agent_type_id, build_id);


--
-- TOC entry 6121 (class 1259 OID 17522)
-- Name: light_history_finish_time_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX light_history_finish_time_i ON public.light_history USING btree (build_finish_time_server);


--
-- TOC entry 6090 (class 1259 OID 17436)
-- Name: metric_key_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX metric_key_index ON public.server_statistics USING btree (metric_key, metric_timestamp);


--
-- TOC entry 5975 (class 1259 OID 17155)
-- Name: mute_info_bt_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX mute_info_bt_ie ON public.mute_info_bt USING btree (build_type_id);


--
-- TOC entry 5993 (class 1259 OID 17191)
-- Name: mute_problem_in_bt_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX mute_problem_in_bt_ie ON public.mute_problem_in_bt USING btree (build_type_id, problem_id, mute_id);


--
-- TOC entry 5990 (class 1259 OID 17185)
-- Name: mute_problem_in_proj_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX mute_problem_in_proj_ie ON public.mute_problem_in_proj USING btree (project_int_id, problem_id, mute_id);


--
-- TOC entry 5984 (class 1259 OID 17173)
-- Name: mute_test_in_bt_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX mute_test_in_bt_ie ON public.mute_test_in_bt USING btree (build_type_id, test_name_id, mute_id);


--
-- TOC entry 5987 (class 1259 OID 17174)
-- Name: mute_test_in_bt_tn_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX mute_test_in_bt_tn_ie ON public.mute_test_in_bt USING btree (test_name_id);


--
-- TOC entry 5980 (class 1259 OID 17166)
-- Name: mute_test_in_proj_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX mute_test_in_proj_ie ON public.mute_test_in_proj USING btree (project_int_id, test_name_id, mute_id);


--
-- TOC entry 5983 (class 1259 OID 17167)
-- Name: mute_test_in_proj_tn_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX mute_test_in_proj_tn_ie ON public.mute_test_in_proj USING btree (test_name_id);


--
-- TOC entry 6091 (class 1259 OID 17442)
-- Name: node_events_created_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX node_events_created_idx ON public.node_events USING btree (created);


--
-- TOC entry 6101 (class 1259 OID 17465)
-- Name: node_locks_node_id_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX node_locks_node_id_idx ON public.node_locks USING btree (node_id, id, lock_type);


--
-- TOC entry 6096 (class 1259 OID 17452)
-- Name: node_tasks_task_state_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX node_tasks_task_state_idx ON public.node_tasks USING btree (task_state);


--
-- TOC entry 5795 (class 1259 OID 16754)
-- Name: notification_events_notifier; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX notification_events_notifier ON public.user_notification_events USING btree (notificator_type);


--
-- TOC entry 5796 (class 1259 OID 16755)
-- Name: notification_events_user_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX notification_events_user_id ON public.user_notification_events USING btree (user_id);


--
-- TOC entry 5831 (class 1259 OID 16836)
-- Name: order_num_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX order_num_idx ON public.test_names USING btree (order_num);


--
-- TOC entry 5819 (class 1259 OID 16815)
-- Name: permanent_t_exp_t_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX permanent_t_exp_t_idx ON public.permanent_tokens USING btree (expiration_time);


--
-- TOC entry 5826 (class 1259 OID 16821)
-- Name: permanent_t_p_pr_id_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX permanent_t_p_pr_id_idx ON public.permanent_token_permissions USING btree (project_id);


--
-- TOC entry 5869 (class 1259 OID 16915)
-- Name: personal_vcs_history_user_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX personal_vcs_history_user_i ON public.personal_vcs_history USING btree (user_id);


--
-- TOC entry 5817 (class 1259 OID 16803)
-- Name: remember_me_secure_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX remember_me_secure_idx ON public.remember_me USING btree (secure);


--
-- TOC entry 5818 (class 1259 OID 16802)
-- Name: remember_me_uk_secure_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX remember_me_uk_secure_idx ON public.remember_me USING btree (user_key, secure);


--
-- TOC entry 5906 (class 1259 OID 17000)
-- Name: removed_b_agent_buildid; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX removed_b_agent_buildid ON public.removed_builds_history USING btree (agent_type_id, build_id);


--
-- TOC entry 5907 (class 1259 OID 16998)
-- Name: removed_b_history_finish_time; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX removed_b_history_finish_time ON public.removed_builds_history USING btree (build_finish_time_server);


--
-- TOC entry 5908 (class 1259 OID 16997)
-- Name: removed_b_start_time_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX removed_b_start_time_index ON public.removed_builds_history USING btree (build_start_time_server);


--
-- TOC entry 5909 (class 1259 OID 16999)
-- Name: removed_b_stats_optimized_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX removed_b_stats_optimized_i ON public.removed_builds_history USING btree (build_type_id, status, is_canceled, branch_name);


--
-- TOC entry 6027 (class 1259 OID 17298)
-- Name: responsibilities_assignee; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX responsibilities_assignee ON public.responsibilities USING btree (responsible_user_id);


--
-- TOC entry 6030 (class 1259 OID 17297)
-- Name: responsibilities_reporter; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX responsibilities_reporter ON public.responsibilities USING btree (reporter_user_id);


--
-- TOC entry 5894 (class 1259 OID 16973)
-- Name: running_state_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX running_state_id ON public.running USING btree (build_state_id);


--
-- TOC entry 6111 (class 1259 OID 17488)
-- Name: server_health_items_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX server_health_items_ie ON public.server_health_items USING btree (report_id, category_id);


--
-- TOC entry 6124 (class 1259 OID 17521)
-- Name: start_time_index_light; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX start_time_index_light ON public.light_history USING btree (build_start_time_server);


--
-- TOC entry 6125 (class 1259 OID 17523)
-- Name: stats_optimized_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX stats_optimized_index ON public.light_history USING btree (build_type_id, status, is_canceled, branch_name);


--
-- TOC entry 5933 (class 1259 OID 17052)
-- Name: test_archive_name_id_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX test_archive_name_id_idx ON public.test_info_archive USING btree (test_name_id);


--
-- TOC entry 6004 (class 1259 OID 17209)
-- Name: test_failure_rate_tn_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX test_failure_rate_tn_idx ON public.test_failure_rate USING btree (test_name_id);


--
-- TOC entry 5946 (class 1259 OID 17076)
-- Name: test_metadataname_name_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX test_metadataname_name_idx ON public.test_metadata USING btree (test_name_id);


--
-- TOC entry 5999 (class 1259 OID 17203)
-- Name: test_muted_mute_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX test_muted_mute_id ON public.test_muted USING btree (mute_id);


--
-- TOC entry 5932 (class 1259 OID 17045)
-- Name: test_name_id_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX test_name_id_idx ON public.test_info USING btree (test_name_id);


--
-- TOC entry 6021 (class 1259 OID 17274)
-- Name: user_build_params_state_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX user_build_params_state_id ON public.user_build_parameters USING btree (build_state_id);


--
-- TOC entry 5853 (class 1259 OID 16874)
-- Name: user_build_types_order_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX user_build_types_order_i ON public.user_build_types_order USING btree (project_int_id);


--
-- TOC entry 5803 (class 1259 OID 16768)
-- Name: user_notif_data_rule_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX user_notif_data_rule_id ON public.user_notification_data USING btree (rule_id);


--
-- TOC entry 5848 (class 1259 OID 16868)
-- Name: user_projects_order_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX user_projects_order_i ON public.user_projects_order USING btree (project_int_id);


--
-- TOC entry 5845 (class 1259 OID 16862)
-- Name: user_projects_visibility_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX user_projects_visibility_i ON public.user_projects_visibility USING btree (project_int_id);


--
-- TOC entry 5788 (class 1259 OID 16734)
-- Name: user_property_key_value_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX user_property_key_value_idx ON public.user_property USING btree (prop_key, locase_value_hash);


--
-- TOC entry 6041 (class 1259 OID 17323)
-- Name: user_roles_p_int_id_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX user_roles_p_int_id_i ON public.user_roles USING btree (project_int_id);


--
-- TOC entry 5799 (class 1259 OID 16759)
-- Name: user_watch_type_pk; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX user_watch_type_pk ON public.user_watch_type USING btree (user_id, notificator_type, watch_type, watch_value);


--
-- TOC entry 5808 (class 1259 OID 16785)
-- Name: usergroup_events_group_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX usergroup_events_group_id ON public.usergroup_notification_events USING btree (group_id);


--
-- TOC entry 5809 (class 1259 OID 16784)
-- Name: usergroup_events_notifier; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX usergroup_events_notifier ON public.usergroup_notification_events USING btree (notificator_type);


--
-- TOC entry 6044 (class 1259 OID 17329)
-- Name: usergroup_roles_p_int_id_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX usergroup_roles_p_int_id_i ON public.usergroup_roles USING btree (project_int_id);


--
-- TOC entry 5813 (class 1259 OID 16789)
-- Name: usergroup_watch_type_pk; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX usergroup_watch_type_pk ON public.usergroup_watch_type USING btree (group_id, notificator_type, watch_type, watch_value);


--
-- TOC entry 5872 (class 1259 OID 16928)
-- Name: vcs_changes_graph_parent_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX vcs_changes_graph_parent_i ON public.vcs_changes_graph USING btree (parent_modification_id);


--
-- TOC entry 6118 (class 1259 OID 17507)
-- Name: vcs_changes_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX vcs_changes_index ON public.vcs_changes USING btree (modification_id);


--
-- TOC entry 5859 (class 1259 OID 16896)
-- Name: vcs_history_date_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX vcs_history_date_i ON public.vcs_history USING btree (register_date);


--
-- TOC entry 5862 (class 1259 OID 16895)
-- Name: vcs_history_root_id_mod_id_i; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX vcs_history_root_id_mod_id_i ON public.vcs_history USING btree (vcs_root_id, modification_id);


--
-- TOC entry 6119 (class 1259 OID 17513)
-- Name: vcs_personal_changes_index; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX vcs_personal_changes_index ON public.personal_vcs_changes USING btree (modification_id);


--
-- TOC entry 5854 (class 1259 OID 16882)
-- Name: vcs_root_instance_parent_idx; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX vcs_root_instance_parent_idx ON public.vcs_root_instance USING btree (parent_id, settings_hash);


--
-- TOC entry 5883 (class 1259 OID 16948)
-- Name: vcs_username_user_ie; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX vcs_username_user_ie ON public.vcs_username USING btree (vcs_name, parent_vcs_root_id, username);


--
-- TOC entry 5800 (class 1259 OID 16760)
-- Name: watch_type_rule_id; Type: INDEX; Schema: public; Owner: teamcity
--

CREATE INDEX watch_type_rule_id ON public.user_watch_type USING btree (rule_id);


-- Completed on 2024-03-13 11:34:32

--
-- PostgreSQL database dump complete
--

