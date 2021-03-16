using ClfApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClfApi.Services
{
    public class UtilService : IUtilService
    {
        public IEnumerable<Clf> BatchToList(Stream stream)
        {
            IList<Clf> clfs = new List<Clf>();

            using (var reader = new StreamReader(stream))
            {
                while (reader.Peek() >= 0)
                {
                    string record = reader.ReadLine();
                    clfs.Add(StringToClf(record));
                }
            }

            return clfs;
        }
        public Clf StringToClf(string str)
        {
            string pattern = @"^(?<Client>\S+)\s+" +
                                @"(?<RfcIdentity>\S+)\s+" +
                                @"(?<UserId>\S+)\s+" +
                                @"\[(?<RequestDate>[^\]]+)\]\s+" +
                                "\"" + @"(?<Method>[A-Z]+)\s+" +
                                @"(?<Request>[^" + "\"" + @"]+)?HTTP/[0-9.]+" + "\"" + @"\s+" +
                                @"(?<StatusCode>[0-9]{3})\s+" +
                                @"(?<ResponseSize>[0-9]+|-)" +
                                @"(\s+" + "\"" + @"(?< Referrer >[^" + "\"]*)\"" + @"\s+" + "\"" + @"(?<UserAgent>[^" + "\"" + "]*)\")?";

            Regex reg = new Regex(pattern);

            Match match = reg.Match(str);

            Clf clf = new Clf
            {
                Client = match.Groups["Client"].ToString(),
                RfcIdentity = match.Groups["RfcIdentity"].ToString(),
                UserId = match.Groups["UserId"].ToString(),
                RequestDate = DateTime.Parse(match.Groups["RequestDate"].ToString()),
                Method = match.Groups["Method"].ToString(),
                Request = match.Groups["Request"].ToString(),
                StatusCode = int.Parse(match.Groups["StatusCode"].ToString()),
                ResponseSize = int.Parse(match.Groups["ResponseSize"].ToString()),
                Referrer = match.Groups["Referrer"].ToString(),
                UserAgent = match.Groups["UserAgent"].ToString()
            };

            /*
            string[] values = str.Split(" ");

            string[] propertyIndexes = { "IpAddress", "RfcIdentity", "UserId", "RequestDate", "Request", "StatusCode", "ResponseSize", "Client", "UserAgent" };

            for (int i = 0; i < values.Length; ++i)
            {
                if (values[i] != "-")
                {
                    PropertyInfo prop = clf.GetType().GetProperty(propertyIndexes[i]);
                    if (prop.PropertyType == typeof(DateTime))
                    {
                        // Removing '[' and ']' from string
                        string value = values[i].Remove(0, 1).Remove(values[i].Length, 1);
                        prop.SetValue(clf, DateTime.Parse(value));
                    }
                    else
                    {
                        prop.SetValue(clf, values[i]);
                    }
                }
            }
            */
            return clf;
        }
    }

    public interface IUtilService
    {
        IEnumerable<Clf> BatchToList(Stream stream);
        Clf StringToClf(string str);
    }
}
