using ClfApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                                @"\[(?<RequestDate>\S+)\s+" + @"(?<UTC>[^\]]+)\]\s+" +
                                "\"" + @"(?<Method>[A-Z]+)\s+" +
                                @"(?<Request>\S+)\s+HTTP/" + @"(?<Protocol>[0-9.]+)" + "\"" + @"\s+" +
                                @"(?<StatusCode>[0-9]{3})\s+" +
                                @"(?<ResponseSize>[0-9]+|-)" +
                                @"(\s+" + "\"" + @"(?<Referrer>[^" + "\"]*)\"" + @"\s+" + "\"" + @"(?<UserAgent>[^" + "\"" + "]*)\")?";

            Regex reg = new Regex(pattern);

            Match match = reg.Match(str);

            Clf clf = new Clf
            {
                Client = match.Groups["Client"].ToString(),
                RfcIdentity = match.Groups["RfcIdentity"].ToString() != "-" ? match.Groups["RfcIdentity"].ToString() : null,
                UserId = match.Groups["UserId"].ToString() != "-" ? match.Groups["UserId"].ToString() : null,
                RequestDate = ClfRequestDate(match.Groups["RequestDate"].ToString(), match.Groups["UTC"].ToString()),
                Method = match.Groups["Method"].ToString(),
                Request = match.Groups["Request"].ToString(),
                Protocol = match.Groups["Protocol"].ToString(),
                StatusCode = int.Parse(match.Groups["StatusCode"].ToString()),
                ResponseSize = match.Groups["ResponseSize"].ToString() != "-" ? int.Parse(match.Groups["ResponseSize"].ToString()) : 0,
                Referrer = match.Groups["Referrer"].ToString() != string.Empty ? match.Groups["Referrer"].ToString() : null,
                UserAgent = match.Groups["UserAgent"].ToString() != string.Empty ? match.Groups["UserAgent"].ToString() : null
            };

            return clf;
        }

        public DateTime ClfRequestDate(string requestDate, string utc)
        {
            // Inserting ':' to separate UTC => -1000 to -10:00
            utc = utc.Insert(3, ":");
            string dateTimeFormat = "dd/MMM/yyyy:HH:mm:ss zzz";

            return DateTime.ParseExact(requestDate + " " + utc, dateTimeFormat, CultureInfo.InvariantCulture);
        }
    }

    public interface IUtilService
    {
        IEnumerable<Clf> BatchToList(Stream stream);
        Clf StringToClf(string str);
        DateTime ClfRequestDate(string requestDate, string utc);
    }
}
