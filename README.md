what to do.

sessions
1. add enum for session status, running, pending and ended.
2. function or background job to check the status of the session status.


tickets
1. add createdAt to the model.






INSERT INTO AgentDocuments (
    AgentsId, 
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
)
SELECT 
    172380 AS AgentsId,
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
FROM AgentDocuments
WHERE AgentsId = 52850;


INSERT INTO AgentDocuments (
    AgentsId, 
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
)
SELECT 
    172384 AS AgentsId,
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
FROM AgentDocuments
WHERE AgentsId = 52850;


INSERT INTO AgentDocuments (
    AgentsId, 
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
)
SELECT 
    172385 AS AgentsId,
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
FROM AgentDocuments
WHERE AgentsId = 52850;


INSERT INTO AgentDocuments (
    AgentsId, 
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
)
SELECT 
    172386 AS AgentsId,
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
FROM AgentDocuments
WHERE AgentsId = 52850;



INSERT INTO AgentDocuments (
    AgentsId, 
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
)
SELECT 
    156713 AS AgentsId, 
    d.DocumentType, 
    d.DocumentUrl, 
    d.DocumentStatus, 
    d.DocumentNumber, 
    d.DocumentIdentityType, 
    d.DocumentDetails, 
    d.RejectReason
FROM AgentDocuments d
WHERE d.AgentsId = 156226;



INSERT INTO AgentDocuments (
    AgentsId, 
    DocumentType, 
    DocumentUrl, 
    DocumentStatus, 
    DocumentNumber, 
    DocumentIdentityType, 
    DocumentDetails, 
    RejectReason
)
SELECT 
    156715 AS AgentsId, 
    d.DocumentType, 
    d.DocumentUrl, 
    d.DocumentStatus, 
    d.DocumentNumber, 
    d.DocumentIdentityType, 
    d.DocumentDetails, 
    d.RejectReason
FROM AgentDocuments d
WHERE d.AgentsId = 156226;



-- script 1
UPDATE Agents
SET SponsorId = 52850,
    SponsorUsername = 'smartmark'
WHERE AgentId IN (172380, 172384, 172385, 172386);


-- script 2
UPDATE Agents
SET KycLevel = 4
WHERE AgentId IN (172380, 172384, 172385, 172386);







