# isbndb
A helper library written in C# that makes connecting to isbndb (an isbn lookup web-service) super simple


//////////////////////////////////////////////////////////////////////////////////////////////////////////

Need to get some information about a book via isbn???
Why screw around with HttpRequests and XML parsing when this library makes everything completely idiot 
proof!  Simply add a reference to the .dll in your C# .net project, then at the top of your class add:

using jordanbleu.isbndb;

Then create a new instance of the isbnRequest object included in the library, supplying two parameters:
The first parameter is your API key, which is created when you create an account on isbndb.net...
THe second parameter is the isbn to lookup (no dashes)

// Example:
isbnRequest request = new isbnRequest ("XXXXXXXX", "1234567890");

In the constructor of that object, the web request is made and the returned xml file is saved.  
You are ready to go!

/////////////////////////////////////////////////////////////////////////////////////////////////////////

Methods / Properties:

request.online - Returns true if the PC was online when the request was made.

request.found - Returns true if the PC was online AND a result was returned

request.GetTitle() - returns the title of the book

request.GetAuthor() - returns the author, or a list of authors in a concatenated string

request.GetPublisher() - returns the publisher

request.GetIsbn10() - returns the isbn10

request.GetIsbn13() - returns the isbn13

request.GetSummary() - gets the summary of the book (if there is one, which there ususally isn't)


///////////////////////////


Eventually I will add the ability to get more information.  I made this library for a college class project, and I somewhat slopped it together in about an hour.  I also plan on setting the returned title/author/etc as a property rather than having a method call to return these values, since that makes more sense with the logic of the code.  
