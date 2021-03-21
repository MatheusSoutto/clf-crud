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
                                @"\[(?<RequestDateTime>[^\]]+)\]\s+" +
                                "\"" + @"(?<Method>[A-Z]+)\s+" +
                                @"(?<Request>\S+)\s+HTTP/" + @"(?<Protocol>[0-9.]+)" + "\"" + @"\s+" +
                                @"(?<StatusCode>[0-9]{3})\s+" +
                                @"(?<ResponseSize>[0-9]+|-)" +
                                @"(\s+" + "\"" + @"(?<Referrer>[^" + "\"]*)\"" + @"\s+" + "\"" + @"(?<UserAgent>[^" + "\"" + "]*)\")?";

            Regex reg = new Regex(pattern);

            Match match = reg.Match(str);

            DateTimeOffset dateTimeOffset = DateTimeOffsetFromString(match.Groups["RequestDateTime"].ToString());

            Clf clf = new Clf
            {
                Client = match.Groups["Client"].ToString(),
                RfcIdentity = match.Groups["RfcIdentity"].ToString() != "-" ? match.Groups["RfcIdentity"].ToString() : null,
                UserId = match.Groups["UserId"].ToString() != "-" ? match.Groups["UserId"].ToString() : null,
                RequestDate = dateTimeOffset.UtcDateTime,
                RequestTime = dateTimeOffset,
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

        public DateTimeOffset DateTimeOffsetFromString(string requestDateTime)
        {
            // Inserting ':' on Offset => "04/Nov/2020:12:16:04 -1000" to "04/Nov/2020:12:16:04 -10:00"
            requestDateTime = requestDateTime.Insert(24, ":");
            string dateTimeFormat = "dd/MMM/yyyy:HH:mm:ss zzz";

            return DateTimeOffset.ParseExact(requestDateTime, dateTimeFormat, CultureInfo.InvariantCulture);
        }

        public void ClfSetRequestDateTimeOffset(ref Clf clf)
        {
            DateTimeOffset result = new DateTimeOffset(DateTime.SpecifyKind(clf.RequestDate, DateTimeKind.Utc));

            clf.RequestTime = result.ToOffset(new TimeSpan(clf.RequestTime.Offset.Hours, 0, 0));
        }

        public void ClfSetRequestDateTimeOffset(ref List<Clf> clfs)
        {
            clfs.ForEach(clf =>
            {
                ClfSetRequestDateTimeOffset(ref clf);
            });
        }

        public void ClfSetRequestDate(ref Clf clf)
        {
            clf.RequestDate = clf.RequestTime.UtcDateTime;
        }
    }

    public interface IUtilService
    {
        IEnumerable<Clf> BatchToList(Stream stream);
        Clf StringToClf(string str);
        DateTimeOffset DateTimeOffsetFromString(string requestTime);
        void ClfSetRequestDateTimeOffset(ref Clf clf);
        void ClfSetRequestDateTimeOffset(ref List<Clf> clfs);
        void ClfSetRequestDate(ref Clf clf);
    }
}
