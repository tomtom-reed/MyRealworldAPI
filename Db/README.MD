Stored procedures are cancer due to DB scaling, but using them as interfaces to trivial statements has the major advantage of schema consistency. 
If the SSDT repo builds successfully then the DB will be consistent with any uses of the DB. 
Except for the stored procedures themselves, but putting a layer of abstraction between the dotnet code and the tables is good. 

"All problems in computer science can be solved by another level of indirection... 
Except for the problem of too many layers of indirection." -- David Wheeler