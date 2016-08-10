/*
 * Jordan Bleu
 * isbndb C# Library
 * CIS310
 * Originally Written: April 27, 2016
 * Version 2.0 - Updated August 9, 2016
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.IO;

namespace jordanbleu.isbndb
{
    /// <summary>
    /// Creating an instance of this class will send an httprequest to the isbndb web service
    /// </summary>
    public class isbnRequest
    {
        private string _apiKey;
        private string _isbn;
        private XmlDocument _response;
        private bool _found;
        private bool _online;

        // properties for book information
        private string _title;
        private string _author;
        private string _publisher;
        private string _summary;
        private string _isbn10;
        private string _isbn13;
        private string _edition;
        
        /// <summary>
        /// The Constructor will create the web request based on your information you provide in order to get the response as XML
        /// </summary>
        /// <param name="my_apikey">Get this from your account page on isbndb.com under "MANAGE KEYS"</param>
        /// <param name="isbn">This is the isbn10 or isbn13 that you are looking for</param>
        public isbnRequest(string my_apikey, string isbn) {
            this.apiKey = my_apikey;
            this.isbn = isbn;
            this.online = true;

            var requestXml = new XmlDocument();

            //Create a new Web Request object to make the GET request
            WebRequest httpRequest = WebRequest.Create("http://www.isbndb.com/api/v2/xml/" + this.apiKey + "/book/" + this.isbn);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "text/xml;";

            try
            {
                var requestStream = httpRequest.GetRequestStream();
                requestXml.Save(requestStream);

                var response = (HttpWebResponse)httpRequest.GetResponse();
                var responseStream = response.GetResponseStream();

                var responseXml = new XmlDocument();
                responseXml.Load(responseStream);
                this.response = responseXml;

            }
            catch (WebException) { // This exception is thrown if we cannot connect
                this.online = false;
                this.found = false;
            } 

            // Use the returned XML document to set our properties
            // The developer who implements this code will only be able to read these properties,
            // not set them
            _title = xmlReadTo("title");
            _author = GetAuthor();
            _publisher = xmlReadTo("publisher_name");
            _summary = xmlReadTo("summary");
            _isbn10 = xmlReadTo("isbn10");
            _isbn13 = xmlReadTo("isbn13");
            _edition = xmlReadTo("edition_info");

        }

        // xmlReadTo should only be used withing the lib
        private string xmlReadTo(string desc) {
            if (this.found && this.online)
            {
                //take our saved xmldocument and convert it to a stream
                StringReader str = new StringReader(this.ToString());

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;

                XmlDocument x = this.response;
                XmlReader xmlIn = XmlReader.Create(str, settings);

                xmlIn.ReadToDescendant(desc);

                return xmlIn.ReadElementContentAsString();
            }
            else
            {
                return "";
            }

        }



        public string title {
            get { return _title; }        
        }

        public string author {
            get { return _author; }
        }

        public string publisher {
            get { return _publisher; }
        }

        public string summary {
            get { return _summary; }
        }

        public string isbn10 {
            get { return _isbn10; }
        }

        public string isbn13 {
            get { return _isbn13; }
        }

        public string edition {
            get { return _edition; }
        }


        // Handles the author getting
        private string GetAuthor()
        {
            if (this.found)
            {
                //take our saved xmldocument and convert it to a stream
                StringReader str = new StringReader(this.ToString());

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;

                XmlDocument x = this.response;
                XmlReader xmlIn = XmlReader.Create(str, settings);

                string ret = "";

                //So for some reason, on certain books in the isbndb page, the author elements don't exist.  
                //This code stops the program from crashing, and returns a blank value. 
                try {
                    if (xmlIn.ReadToDescendant("author_data"))
                    {
                        do
                        {
                            xmlIn.ReadStartElement("author_data");
                            xmlIn.ReadElementContentAsString();
                            ret += xmlIn.ReadElementContentAsString() + "; ";
                        } while (xmlIn.ReadToNextSibling("author_data"));

                        //remove the semicolon and space at the end
                        ret = ret.Substring(0, ret.Length - 2);
                    }
                    
                }
                catch(InvalidOperationException){
                    ret = "";
                }


                return ret;
            }
            else { return ""; }
        }


        #region properties
        public string apiKey{
            get {
                return _apiKey;
            }
            set {
                _apiKey = value;
            }
        }

        public string isbn {
            get {
                return _isbn;
            }
            set {
                _isbn = value;
            }
        }

        public XmlDocument response
        {
            get
            {
                return _response;
            }
            set
            {
                _response = value;
            }
        }

        public bool online {
            get { return _online; }
            set { _online = value; }
        }

        public bool found { 
            get {

                bool ret; //if we are offline, then we didn't find the data

                if (this.online)
                {
                    StringReader str = new StringReader(this.ToString()); //Turns the reference to our XmlDocument into a data stream

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreComments = true;
                    settings.IgnoreWhitespace = true;

                    XmlDocument x = this.response;
                    XmlReader xmlIn = XmlReader.Create(str, settings);

                    xmlIn.ReadToDescendant("error"); //So basically we are checking for the existence of this tag
                    try
                    {
                        xmlIn.ReadElementContentAsString();
                        ret = false;
                    }
                    catch (InvalidOperationException)
                    { //this happens when the element is not found, which is good
                        ret = true;
                    }
                }
                else ret = false;

                return ret;
            }

            set { _found = value; }

        }


        #endregion  

      
        #region overrides
        public override string ToString()
        {
            if (this.online == true)
            {
                return this.response.OuterXml;
            }
            else { return "isbnRequest: Connection Error"; }

        }//ToString() will output the Xml Response as text

        public override bool Equals(object obj)
        {
            if (obj is isbnRequest && obj != null)
            {
                isbnRequest that = (isbnRequest)obj;
                if (this.GetAuthor() == that.GetAuthor() &&
                    this.isbn10 == that.isbn10 &&
                    this.isbn13 == that.isbn13 &&
                    this.publisher == that.publisher &&
                    this.title == that.title &&
                    this.found == true &&
                    that.found== true)
                {
                    return true;
                }
                else return false;
            }
            else return false;    
        }//Equals() will check if the requests returned the same exact data

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        #endregion
    }
}
