-- Table: public.Clf

-- DROP TABLE public."Clf";

CREATE TABLE public."Clf"
(
    "Id" uuid NOT NULL,
    "Client" character varying(15) COLLATE pg_catalog."default" NOT NULL,
    "RfcIdentity" character varying(50) COLLATE pg_catalog."default",
    "UserId" character varying(50) COLLATE pg_catalog."default",
    "RequestDate" timestamp without time zone NOT NULL,
    "RequestTime" time with time zone NOT NULL,
    "Method" character varying(10) COLLATE pg_catalog."default" NOT NULL,
    "Request" character varying(200) COLLATE pg_catalog."default" NOT NULL,
    "Protocol" character varying(4) COLLATE pg_catalog."default" NOT NULL,
    "StatusCode" integer NOT NULL,
    "ResponseSize" integer,
    "Referrer" character varying(200) COLLATE pg_catalog."default",
    "UserAgent" character varying(200) COLLATE pg_catalog."default",
    CONSTRAINT "Clf_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE public."Clf"
    OWNER to postgres;