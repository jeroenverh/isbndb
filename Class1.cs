/*
 * Jordan Bleu
 * isbndb C# Library
 * CIS310
 * April 27, 2016
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
            catch (WebException) {
                this.online = false;
                this.found = false;
            }
            
  
            
            



         
        }

        /// <summary>
        /// Call this method to get the Title of your result
        /// </summary>
        /// <returns>The title as a string, or -- if nothing was found</returns>
        public string GetTitle() {
            if (this.found)
            {
                //take our saved xmldocument and convert it to a stream
                StringReader str = new StringReader(this.ToString());

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;

                XmlDocument x = this.response;
                XmlReader xmlIn = XmlReader.Create(str, settings);

                xmlIn.ReadToDescendant("title");


                return xmlIn.ReadElementContentAsString();
            } else { return  "";}
        }

        /// <summary>
        /// Call this method to get the author of your result
        /// </summary>
        /// <returns>The author name as a string, or nothing if nothing was found</returns>
        public string GetAuthor()
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


        /// <summary>
        /// Call this method to get the publisher of your result
        /// </summary>
        /// <returns>The Publisher name as a string, or nothing if not found</returns>
        public string GetPublisher()
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

                xmlIn.ReadToDescendant("publisher_name");


                return xmlIn.ReadElementContentAsString();
             } else { return  "";}
        }


        /// <summary>
        /// Call this method to get the summary of your result
        /// </summary>
        /// <returns>the summary as a string, or nothing if not found</returns>
        public string GetSummary()
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

                xmlIn.ReadToDescendant("summary");


                return xmlIn.ReadElementContentAsString();
            } else { return  "";}
        }

        public string GetIsbn10()
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

                xmlIn.ReadToDescendant("isbn10");


                return xmlIn.ReadElementContentAsString();
             } else { return  "";}
        }

        public string GetIsbn13()
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

                xmlIn.ReadToDescendant("isbn13");


                return xmlIn.ReadElementContentAsString();
            } else { return  "";}
        }


        public string GetEdition()
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

                xmlIn.ReadToDescendant("edition_info");


                return xmlIn.ReadElementContentAsString();
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

        public bool found { //this property is readonly
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
                    this.GetIsbn10() == that.GetIsbn10() &&
                    this.GetIsbn13() == that.GetIsbn13() &&
                    this.GetPublisher() == that.GetPublisher() &&
                    this.GetTitle() == that.GetTitle() &&
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
