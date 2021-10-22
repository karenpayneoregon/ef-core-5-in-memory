/*
    This query was generated using ToQueryString
    https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.entityframeworkqueryableextensions.toquerystring?view=efcore-5.0

*/
SELECT [c].[CustomerIdentifier], [c].[City], [c].[CompanyName], [c].[ContactId], [c].[ContactTypeIdentifier], [c].[CountryIdentifier], [c].[Fax], [c].[ModifiedDate], [c].[Phone], [c].[PostalCode], [c].[Region], [c].[Street], [c0].[CountryIdentifier], [c0].[Name], [c1].[ContactId], [c1].[ContactTypeIdentifier], [c1].[FirstName], [c1].[LastName], [t].[id], [t].[ContactId], [t].[PhoneNumber], [t].[PhoneTypeIdentifier], [t].[PhoneTypeIdenitfier], [t].[PhoneTypeDescription]
FROM [Customers] AS [c]
LEFT JOIN [Countries] AS [c0] ON [c].[CountryIdentifier] = [c0].[CountryIdentifier]
LEFT JOIN [Contacts] AS [c1] ON [c].[ContactId] = [c1].[ContactId]
LEFT JOIN (
    SELECT [c2].[id], [c2].[ContactId], [c2].[PhoneNumber], [c2].[PhoneTypeIdentifier], [p].[PhoneTypeIdenitfier], [p].[PhoneTypeDescription]
    FROM [ContactDevices] AS [c2]
    LEFT JOIN [PhoneType] AS [p] ON [c2].[PhoneTypeIdentifier] = [p].[PhoneTypeIdenitfier]
) AS [t] ON [c1].[ContactId] = [t].[ContactId]
ORDER BY [c].[CustomerIdentifier], [c0].[CountryIdentifier], [c1].[ContactId], [t].[id], [t].[PhoneTypeIdenitfier]