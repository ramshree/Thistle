using Cassandra;

namespace ConnectWithCassandra
{
    public abstract class BaseDataAccess
    {
        #region Variables
        private static Cluster cluster;
        public static ISession session { get; set; }
        #endregion

        #region Constructors
        public BaseDataAccess()
        {
            SetCluster();
        }
        #endregion

        #region Public Methods
        public ISession GetSession(string keyspace)
        {
            if (cluster == null)
            {
                SetCluster();
                session = cluster.Connect();
            }
            else if (session == null)
            {
                session = cluster.Connect(keyspace);
            }

            return session;
        }

        #endregion

        #region Private Methods
        private void SetCluster()
        {
            if (cluster == null)
            {
                cluster = Connect();
            }
        }

        private Cluster Connect()
        {
            //string user = getAppSetting("user");
            //string pwd = getAppSetting("password");
            //System.Net.IPAddress node = System.Net.IPAddress.Parse(getAppSetting("cassandraDevNodes").Split(',').First());

            QueryOptions queryOptions = new QueryOptions().SetConsistencyLevel(ConsistencyLevel.LocalQuorum);

            //Cluster cluster = Cluster.Builder()
            //.AddContactPoints(node)
            //.WithCredentials(user, pwd)
            //.WithQueryOptions(queryOptions)
            //.Build();

            return Cluster.Builder()
                    .WithCredentials("app", "Password1")
                    .AddContactPoints(System.Net.IPAddress.Parse("10.227.154.242"))
                    .WithQueryOptions(queryOptions)
                    .Build();
            // Connect to the nodes using a keyspace
            //var session = cluster.Connect("simrevamp");

            // return cluster;
        }

        private string getAppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
        #endregion
    }
}
