﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity.Entity;
using HtmlAgilityPack;

namespace Resources
{
    public static class HemaRatingsHelper
    {

        public static async Task<bool> SyncFigthersAsync()
        {
            try
            {
                if (!SqlDal_HemaRatings.DeleteFighters())
                    return false;

                string fightersUrl = ConfigurationManager.AppSettings["HemaRatingsFightersUrl"];

                var response = await GetResponse(fightersUrl).ConfigureAwait(false);

                List<HtmlNode> figtherNodes = GetNodes(response);

                List<HemaRatingsFighterEntity> hemaFigthers = new List<HemaRatingsFighterEntity>();

                int i = 1;

                foreach (var node in figtherNodes)
                {

                    var li = node.Descendants("td").ToList();

                    if (li.Count > 0)
                    {
                        var name_surname = li[0].InnerText;
                        string nationality = GetNationality(li);
                        var figtherId = GetId(li);
                        int clubId = GetClubId(li);

                        SqlDal_HemaRatings.InsertFightersIntoDB(new HemaRatingsFighterEntity
                        {
                            Id = figtherId,
                            IdClub = clubId,
                            Name = name_surname.Replace("'", "''"),
                            Nationality = nationality.Replace("'", "''")
                        });
                    }
                }
                //return SqlDal_HemaRatings.InsertFightersIntoDB(hemaFigthers);            
                return true;

            }
            catch
            {
                return false;
            }

        }


        public static async Task<bool> SyncClubsAsync()
        {
            try
            {
                if (!SqlDal_HemaRatings.DeleteClubs())
                    return false;

                string clubsUrl = ConfigurationManager.AppSettings["HemaRatingsClubsUrl"];

                var response = await GetResponse(clubsUrl).ConfigureAwait(false);

                List<HtmlNode> clubNodes = GetNodes(response);

                List<HemaRatingsClubEntity> hemaClubs = new List<HemaRatingsClubEntity>();

                foreach (var node in clubNodes)
                {
                    var li = node.Descendants("td").ToList();

                    if (li.Count > 0)
                    {
                        var clubName = li[0].InnerText.Replace("\r\n", "").Trim();
                        var clubId = GetId(li);
                        var country = GetCountry(li);
                        var state = GetState(li);
                        var city = GetCity(li);

                        SqlDal_HemaRatings.InsertClubsIntoDB(new HemaRatingsClubEntity
                        {
                            Id = clubId,
                            Name = clubName.Replace("'", "''"),
                            Country = country.Replace("'", "''"),
                            State = state.Replace("'", "''"),
                            City = city.Replace("'", "''")
                        });
                    }

                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool SyncFightersBetweenDBs()
        {
            return SqlDal_HemaRatings.SyncWithLocalFightersId();
        }


        #region Parsing
        private static List<HtmlNode> GetNodes(byte[] response)
        {
            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = System.Net.WebUtility.HtmlDecode(source);
            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(source);

            List<HtmlNode> figtherNodes = resultat.DocumentNode.Descendants().
                Where(x => (x.Name == "tr")).ToList();
            return figtherNodes;
        }

        private static async System.Threading.Tasks.Task<byte[]> GetResponse(string url)
        {
            HttpClient http = new HttpClient();
            
            var response = await http.GetByteArrayAsync(url).ConfigureAwait(false);
            
            return response;
        }

        private static int GetId(List<HtmlNode> li)
        {
            try
            {
                return Convert.ToInt32(li[0].Descendants("a").ToList()[0].GetAttributeValue("href", null).Split('/').ElementAt(3));
            }
            catch
            {
                return -1;
            }
        }

        private static int GetClubId(List<HtmlNode> li)
        {
            try
            {
                return Convert.ToInt32(li[2].Descendants("a").ToList()[0].GetAttributeValue("href", null).Split('/').ElementAt(3));
            }
            catch
            {
                return -1;
            }
        }

        private static string GetNationality(List<HtmlNode> li)
        {
            try
            {
                return li[1].InnerText;
            }
            catch
            {
                return "";
            }
        }

        private static string GetCity(List<HtmlNode> li)
        {
            try
            {
                return li[3].InnerText;
            }
            catch
            {
                return "";
            }
        }

        private static string GetState(List<HtmlNode> li)
        {
            try
            {
                return li[2].InnerText;
            }
            catch
            {
                return "";
            }
        }

        private static string GetCountry(List<HtmlNode> li)
        {
            try
            {
                return li[1].InnerText;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        

    }

}
