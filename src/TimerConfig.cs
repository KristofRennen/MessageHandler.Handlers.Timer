using System.Configuration;
using MessageHandler;

namespace Timer
{
    /// <summary>
    /// Configuration section for twitter stream config
    /// </summary>
    [HandlerConfiguration]
    public class TimerConfig : ConfigurationSection
    {
        /// <summary>
        /// A Body To Send
        /// </summary>
        [ConfigurationProperty("Body", IsRequired = true)]
        public string Body
        {
            get { return this["Body"] as string; }
            set { this["Body"] = value; }
        }

        /// <summary>
        /// A Connection String
        /// </summary>
        [ConfigurationProperty("ConnectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return this["ConnectionString"] as string; }
            set { this["ConnectionString"] = value; }
        }

        /// <summary>
        /// A Connection String
        /// </summary>
        [ConfigurationProperty("Interval", IsRequired = true)]
        public string Interval
        {
            get { return this["Interval"] as string; }
            set { this["Interval"] = value; }
        }

    }
}
