using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Store;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.Search.Function;
using Lucene.Net.QueryParsers;
using Lucene.Net.Util;
using Lucene.Net.Spatial;
using Lucene.Net.Spatial.Prefix;
using Lucene.Net.Spatial.Prefix.Tree;
using Lucene.Net.Spatial.Util;
using Lucene.Net.Spatial.Queries;
using Lucene.Net.Spatial.Vector;
using Spatial4n.Core.Context;
using Spatial4n.Core.Shapes;
using Spatial4n.Core.Shapes.Impl;
using Spatial4n.Core.Distance;
using System.Web;
using System.IO;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects;

namespace HristoEvtimov.Websites.Work.WorkSearch
{
    public class JobSearch
    {
        public enum pathToIndex { SearchIndex1, SearchIndex2 };
        
        public const Lucene.Net.Util.Version luceneVersion = Lucene.Net.Util.Version.LUCENE_30;
        public enum SortBy { Relevance, StartDate, Distance };

        public bool RecreateJobIndex(List<JobSearchIndex> jobPosts, pathToIndex path)
        {
            bool result = false;

            SpatialContext spatialContext = SpatialContext.GEO;
            SpatialPrefixTree spatialGrid = new GeohashPrefixTree(spatialContext, 12);
            SpatialStrategy spatialStrategy = new RecursivePrefixTreeStrategy(spatialGrid, "position");

            IndexWriter luceneWriter = null;
            try
            {
                luceneWriter = new IndexWriter(FSDirectory.Open(HttpContext.Current.Server.MapPath("~/" + path.ToString())),
                    new StopAnalyzer(luceneVersion),
                    true,
                    IndexWriter.MaxFieldLength.UNLIMITED);

                foreach (JobSearchIndex jobPost in jobPosts)
                {
                    Document luceneDocument = new Document();

                    if (!String.IsNullOrEmpty(jobPost.Job.Benefits))
                    {
                        luceneDocument.Add(new Field("Benefits", jobPost.Job.Benefits, Field.Store.NO, Field.Index.ANALYZED));
                    }
                    if (jobPost.JobCompany != null)
                    {
                        if (!String.IsNullOrEmpty(jobPost.JobCompany.Name) && !jobPost.Job.IsAnonymousAd)
                        {
                            luceneDocument.Add(new Field("Company", jobPost.JobCompany.Name, Field.Store.YES, Field.Index.ANALYZED));
                        }
                    }
                    if (!String.IsNullOrEmpty(jobPost.Job.Description))
                    {
                        luceneDocument.Add(new Field("Description", jobPost.Job.Description, Field.Store.NO, Field.Index.ANALYZED));
                    }
                    if (jobPost.JobEmploymentType != null)
                    {
                        if (!String.IsNullOrEmpty(jobPost.JobEmploymentType.Name))
                        {
                            luceneDocument.Add(new Field("EmploymentType", jobPost.JobEmploymentType.Name, Field.Store.NO, Field.Index.ANALYZED));
                        }
                    }
                    luceneDocument.Add(new Field("JobPostId", jobPost.Job.JobPostId.ToString(), Field.Store.YES, Field.Index.NO));
                    if (!String.IsNullOrEmpty(jobPost.Job.Keywords))
                    {
                        luceneDocument.Add(new Field("Keywords", jobPost.Job.Keywords, Field.Store.NO, Field.Index.ANALYZED));
                    }
                    if (!String.IsNullOrEmpty(jobPost.Job.Location))
                    {
                        luceneDocument.Add(new Field("Location", jobPost.Job.Location, Field.Store.YES, Field.Index.ANALYZED));
                    }
                    if (!String.IsNullOrEmpty(jobPost.Job.Position))
                    {
                        luceneDocument.Add(new Field("Position", jobPost.Job.Position, Field.Store.YES, Field.Index.ANALYZED));
                    }
                    if (!String.IsNullOrEmpty(jobPost.Job.Requirements))
                    {
                        luceneDocument.Add(new Field("Requirements", jobPost.Job.Requirements, Field.Store.NO, Field.Index.ANALYZED));
                    }
                    if (jobPost.Job.StartDate != null)
                    {
                        luceneDocument.Add(new Field("StartDate", jobPost.Job.StartDate.ToString(), Field.Store.YES, Field.Index.NO));
                        //save date as a number for sorting.
                        luceneDocument.Add(new Field("StartDateInteger", ((DateTime)jobPost.Job.StartDate).ToString("yyyyMMdd"), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
                    }
                    if (!String.IsNullOrEmpty(jobPost.Job.Title))
                    {
                        luceneDocument.Add(new Field("Title", jobPost.Job.Title, Field.Store.YES, Field.Index.ANALYZED));
                    }

                    if (jobPost.Zip != null)
                    {
                        Shape spatialShape = spatialContext.MakePoint((double)jobPost.Zip.Longitude, (double)jobPost.Zip.Lattitude);
                        AbstractField[] indexableSpatialFields = spatialStrategy.CreateIndexableFields(spatialShape);
                        foreach (AbstractField indexableSpatialField in indexableSpatialFields)
                        {
                            luceneDocument.Add(indexableSpatialField);
                        }
                        //TODO: SAVE Coordinates only saves rouded to one decimal
                        //luceneDocument.Add(new Field("Coordinates", spatialShape.ToString(), Field.Store.YES, Field.Index.NO));
                    }
                    if (!String.IsNullOrEmpty(jobPost.Job.SeUrl))
                    {
                        luceneDocument.Add(new Field("SeUrl", jobPost.Job.SeUrl, Field.Store.YES, Field.Index.NO));
                    }
                    if (!String.IsNullOrEmpty(jobPost.Job.SeDescription))
                    {
                        luceneDocument.Add(new Field("SeDescription", jobPost.Job.SeDescription, Field.Store.YES, Field.Index.NO));
                    }

                    //add categories
                    foreach (JobPostCategory jobPostCategory in jobPost.Job.JobPostCategories)
                    {
                        NumericField categoryField = new NumericField("Category", Field.Store.YES, true);
                        categoryField.SetIntValue(jobPostCategory.CategoryId);
                        luceneDocument.Add(categoryField);
                    }

                    luceneWriter.AddDocument(luceneDocument);
                }
                result = true;
            }
            catch (System.Exception ex)
            {
                result = false;
            }
            finally
            {
                if (luceneWriter != null)
                {
                    luceneWriter.Optimize();
                    luceneWriter.Dispose();
                }
            }
            return result;
        }

        public List<JobSearchResult> Search(string searchInput, int categoryId, double lat, double lon, double radius, SortBy sort, int page, int pageSize, out int totalNumberOfResults)
        {
            SpatialContext spatialContext = SpatialContext.GEO;
            SpatialPrefixTree spatialGrid = new GeohashPrefixTree(spatialContext, 12);
            SpatialStrategy spatialStrategy = new RecursivePrefixTreeStrategy(spatialGrid, "position");

            SettingManager settingManager = new SettingManager();
            pathToIndex path = pathToIndex.SearchIndex1;
            Setting settingActiveIndex = settingManager.GetSetting(SettingManager.SettingNames.ActiveIndex);
            if (settingActiveIndex != null)
            {
                Enum.TryParse<pathToIndex>(settingActiveIndex.SettingValue, out path);
            }

            FSDirectory luceneDirectory = FSDirectory.Open(HttpContext.Current.Server.MapPath("~/" + path.ToString()));
            IndexReader luceneReader = IndexReader.Open(luceneDirectory, true);
            IndexSearcher luceneSearcher = new IndexSearcher(luceneReader);

            MultiFieldQueryParser luceneQueryParser = new MultiFieldQueryParser(luceneVersion,
                new string[] { "Description", "Title", "Location", "Company", "Keywords" },
                new StopAnalyzer(luceneVersion),
                new Dictionary<string, float>() { { "Keywords", 10 },
                    {"Description", 5},
                    {"Title", 5},
                    {"Location", 5},
                    {"Company", 5},
                }
            );

            //filter user input and prepate a query.
            BooleanQuery luceneQuery = new BooleanQuery();
            searchInput = searchInput.Trim();
            searchInput = QueryParser.Escape(searchInput);
            if (!String.IsNullOrEmpty(searchInput))
            {
                luceneQuery.Add(luceneQueryParser.Parse(searchInput), Occur.MUST);
            }
            else
            {
                luceneQuery.Add(new MatchAllDocsQuery(), Occur.MUST);
            }

            //add category filter
            if (categoryId > 0)
            {
                luceneQuery.Add(NumericRangeQuery.NewIntRange("Category", categoryId, categoryId, true, true), Occur.MUST);
            }

            //sort
            Sort luceneSort = null;
            switch(sort)
            {
                case SortBy.Distance:
                    break;
                case SortBy.StartDate :
                    luceneSort = new Sort(new SortField("StartDateInteger", SortField.LONG, true));
                    break;
                case SortBy.Relevance :
                    //sort by relevance. this is by default. no sort
                    break;
            }
            
            //filter
            Filter luceneFilter = null;
            if (lat != 0 && lon != 0)
            {
                //run search inclusing spatial
                radius = DistanceUtils.Dist2Degrees(radius, DistanceUtils.EARTH_MEAN_RADIUS_MI);

                Point spatialCenterPoint = new PointImpl(lon, lat, spatialContext);
                Shape spatialCircle = new CircleImpl(spatialCenterPoint, radius, spatialContext);

                SpatialArgs spatialArgs = new SpatialArgs(SpatialOperation.Intersects, spatialCircle);
                luceneFilter = spatialStrategy.MakeFilter(spatialArgs);
            }

            //run search
            TopDocs luceneTopDocs = null;
            if (luceneSort != null)
            {
                luceneTopDocs = luceneSearcher.Search(luceneQuery, luceneFilter, (page * pageSize), luceneSort);
            }
            else
            {
                luceneTopDocs = luceneSearcher.Search(luceneQuery, luceneFilter, (page * pageSize));
            }

            totalNumberOfResults = luceneTopDocs.TotalHits; 
            
            ScoreDoc[] luceneScoreDocs = luceneTopDocs.ScoreDocs;


            //add lucene search results to an object
            List<JobSearchResult> results = new List<JobSearchResult>();
            for (int i = ((page - 1) * pageSize); (i < luceneScoreDocs.Length) && (i < page * pageSize); i++)
            {
                ScoreDoc luceneScoreDoc = luceneScoreDocs[i];

                JobSearchResult result = new JobSearchResult();
                result.Title = luceneSearcher.Doc(luceneScoreDoc.Doc).Get("Title");
                result.JobPostId = Int32.Parse(luceneSearcher.Doc(luceneScoreDoc.Doc).Get("JobPostId"));
                result.StartDate = DateTime.Parse(luceneSearcher.Doc(luceneScoreDoc.Doc).Get("StartDate"));
                result.Location = luceneSearcher.Doc(luceneScoreDoc.Doc).Get("Location");
                result.SeUrl = luceneSearcher.Doc(luceneScoreDoc.Doc).Get("SeUrl");
                result.SeDescription = luceneSearcher.Doc(luceneScoreDoc.Doc).Get("SeDescription");
                results.Add(result);
            }
            return results;
        }
    }    
}
