﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Avalara.AvaTax.RestClient
{
    /// <summary>
    /// This class contains methods to assist with content for offline calculations. 
    /// 
    /// Store the rate or content files locally for those jurisidictions in which you have nexus. 
    /// </summary>
    public static class AvaTaxOfflineHelper
    {
        /// <summary>
        /// A Dictionary of AvaTax tax rate model objects. The dictionary key 
        /// is the ZIP code for which you wish to retrieve a TaxRateModel, if
        /// it has been downloaded.
        /// </summary>
        public static Dictionary<string, TaxRateModel> RatesByZip;

        /// <summary>
        /// Downloads and stores a ZIP code TaxRateModel object and stores it in the location
        /// designated. Call this method with the Z
        /// </summary>
        /// <param name="client"></param>
        /// <param name="region"></param>
        /// <param name="zip"></param>
        /// <param name="path">The fully qualified path where the file will be stored.</param>
        public static void StoreZipRateContent(AvaTaxClient client, string region, List<string> zips, string path)
        {
            foreach (string zip in zips) {
                //Call rate by ZIP endpoint.
                var rateFile = client.TaxRatesByPostalCode(region, zip);

                //Save the rate by ZIP file in the local ZIP folder.
                WriteZipRateFile(rateFile, zip, path);
            }
        }

        /// <summary>Verifies that the local ZIP-only rate file available.</summary>
        /// <param name="zip">The ZIP code for which you want to verify is available locally.</param>
        /// <param name="path">The path where you stored ZIP-only files.</param>
        /// <returns>bool indicating whether the ZIP rate file is present.</returns>
        public static bool VerifyLocalZipRateAvailable(string zip, string path)
        {
            return File.Exists(string.Format("{0}{1}.json", path, zip));
        }

        /// <summary>
        /// Gets the tax rate information for ZIP centroid and stores it locally.
        /// </summary>
        /// <param name="zip">The ZIP code for which you want a ZIP-only tax rate.</param>
        /// <param name="path">The path where you stored ZIP-only files.</param>
        /// <returns>The tax rate model object for the requested ZIP, if available</returns>
        public static TaxRateModel GetTaxRateByZip(string zip, string path)
        {
            TaxRateModel zipRate = null;            
            if (RatesByZip == null) {
                RatesByZip = new Dictionary<string, TaxRateModel>();
            }

            //First see if the ZIP rate file is available in the dictionary.
            if (RatesByZip.ContainsKey(zip)) {
                zipRate = RatesByZip[zip];
            } else if (VerifyLocalZipRateAvailable(zip, path)) {
                RatesByZip.Add(zip, ReadZipRateFile(zip, path));
                zipRate = RatesByZip[zip];
            }

            return zipRate;         
        }



        /// <summary>Writes the ZIP rate file to the designated location.</summary>
        /// <param name="zipRate">The ZIP rate object to store locally.</param>
        /// <param name="zip">The ZIP code of the rate object.</param>
        /// <param name="path">The path in which to store the file.</param>
        private static void WriteZipRateFile(TaxRateModel zipRate, string zip, string path)
        {
            TextWriter writer = null;

            try {
                var content = JsonConvert.SerializeObject(zipRate);
                writer = new StreamWriter(string.Format("{0}{1}.json", path, zip));
                writer.Write(content);
            }
            finally {
                if (writer != null) {
                    writer.Flush();
                    writer.Close();      
                }
            }
        }

        private static TaxRateModel ReadZipRateFile(string zip, string path)
        {
            TextReader reader = null;

            try {
                reader = new StreamReader(string.Format("{0}{1}.json", path, zip));
                var contents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<TaxRateModel>(contents);
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
            }
        }
    }
}