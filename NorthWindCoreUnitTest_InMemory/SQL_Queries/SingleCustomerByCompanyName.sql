--- used for validating EF Core
--- replace dummy param with a real one for running here and remember to reset else the query will
--- fail in code

SELECT C.CustomerIdentifier, 
       C.CompanyName, 
       C.City, 
       C.PostalCode, 
       Contacts.ContactId, 
       Countries.CountryIdentifier, 
       Countries.Name AS Country, 
       C.Phone AS [Customer Phone], 
       Devices.PhoneTypeIdentifier, 
       Devices.PhoneNumber AS [Contact Phone], 
       C.ModifiedDate, 
       Contacts.FirstName, 
       Contacts.LastName
FROM Customers AS C
     INNER JOIN ContactType AS CT ON C.ContactTypeIdentifier = CT.ContactTypeIdentifier
     INNER JOIN Countries ON C.CountryIdentifier = Countries.CountryIdentifier
     INNER JOIN Contacts ON C.ContactId = Contacts.ContactId
     INNER JOIN ContactDevices AS Devices ON Contacts.ContactId = Devices.ContactId
WHERE CustomerIdentifier = @CustomerIdentifier