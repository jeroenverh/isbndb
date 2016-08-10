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
isbnRequest request = new isbnRequest ("ABCS12345", "1234567891111");

In the constructor of that object, the web request is made and the returned xml file is saved.  
All of this happens behind the scenes and all you need to worry about is grabbing and using your data simply by
reading the properties of your object instance!

/////////////////////////////////////////////////////////////////////////////////////////////////////////

To get back the information, simply check the properties of the request object!

request.title = returns the title as a string
request.publisher = returns the publisher as a string
request.author = returns the author as a string 
request.summary = returns the summary as a string
request.isbn10 = returns the isbn10 for your book
request.isbn13 = returns the isbn13 for your book
request.edition = returns the edition information for your book


You can also use request.found, which returns true if there was a result from the lookup, 
or request.online, which returns true if the PC was online when the request was made.  

///////////////////////////

// Simple console application example

 static void Main(string[] args)
        {
            //example isbns
            // murach book: 9781890774721
            // 
            string apiKey = "ABCD1234";//<-------------------------------------- Paste your API key here (without curly  braces)
            string isbn = "9781890774721";//<---------------------------------------------- Change this number to query other ISBNs
            isbnRequest myRequest = new isbnRequest(apiKey, isbn);

            //You can check if you were online at the time the request was made...
            //but only if you want to.
            if (myRequest.online == false)
            {
                Console.WriteLine("You are offline, which is probably why the ISBN wasn't found.");
                Console.WriteLine("What are you ever going to do with your life?");
            }

            //Always check to see if the request found something
                if (myRequest.found == false) { 
                    Console.WriteLine("------- THAT ISBN WAS NOT FOUND ----------- "); 
                }else{
                    Console.WriteLine("Title --> " + myRequest.title);
                    Console.WriteLine("Author --> " + myRequest.author);
                    Console.WriteLine("Publisher --> " + myRequest.publisher);
                    Console.WriteLine("ISBN 10 -->" + myRequest.isbn10);
                    Console.WriteLine("ISBN 13 -->" + myRequest.isbn13);
                    Console.WriteLine("Edition Info -->" + myRequest.edition);
                    Console.WriteLine("..........................................................");
                    Console.WriteLine("Summary --> " + myRequest.summary);
                
                }
                Console.WriteLine("Press a key...");
            Console.ReadKey();
        }



////////////////////////////


 I made this library for a college class project, and I slapped it together in about an hour.  This
 version is vastly improved from the old version.  In the future, I will probably add more properties 
 for book information.
 
 
 
 
 Thank you!
