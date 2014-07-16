namespace Commercan.GoogleAnalytics.Models
{
    public class SettingsRecord
    {
        public virtual int Id { get; set; }
        public virtual bool Enable { get; set; }
        public virtual bool UseUniversalTracking { get; set; }
        public virtual string GoogleAnalyticsKey { get; set; }
        public virtual string DomainName { get; set; }
    }
}