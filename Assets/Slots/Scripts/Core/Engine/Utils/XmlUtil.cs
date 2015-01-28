using UnityEngine; 
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text;

namespace UnitySlot {
    public class XmlUtil {
        /// <summary>
        /// Deserialize  the specified filePath.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <typeparam name="T">Deserialized object</typeparam>
        public static T Deserialize<T> (string filePath) {
            TextAsset textAsset = (TextAsset)Resources.Load (filePath, typeof(TextAsset));
            XmlSerializer serializer = new XmlSerializer (typeof(T));
            StringReader stringReader = new StringReader (textAsset.ToString ());
            XmlTextReader xmlReader = new XmlTextReader (stringReader);
            T obj = (T)serializer.Deserialize (xmlReader);
            xmlReader.Close ();
            stringReader.Close ();
            return obj;
        }

        public static void Serialize<T> (string filePath, T obj) {
            XmlSerializer serializer = new XmlSerializer (typeof(T));
            Stream stream = new FileStream (filePath, FileMode.Create);
            var xmlnsEmpty = new XmlSerializerNamespaces ();
            xmlnsEmpty.Add ("", "");
            serializer.Serialize (stream, obj, xmlnsEmpty);
            stream.Close ();
        }
    }
}

