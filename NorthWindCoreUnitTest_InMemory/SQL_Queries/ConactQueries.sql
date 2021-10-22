SELECT C.ContactId, 
       C.FirstName, 
       C.LastName, 
       C.ContactTypeIdentifier, 
       CT.ContactTitle
FROM Contacts AS C
     INNER JOIN ContactType AS CT ON C.ContactTypeIdentifier = CT.ContactTypeIdentifier
WHERE C.ContactTypeIdentifier IN(7, 12);