using System;

namespace StatefulAggregatePOC.Domain
{
    public class Job
    {
        private readonly Guid _id;

        private readonly DateTime _startDate;

        private DateTime? _endDate;

        private string _jobTitle;

        public Job(DateTime startDate, string jobTitle)
        {
            _id = Guid.NewGuid();
            _startDate = startDate;
            _jobTitle = jobTitle;
        }

        internal Job(JobState jobState)
        {
            _id = jobState.Id;
            _startDate = jobState.StartDate;
            _endDate = jobState.EndDate;
            _jobTitle = jobState.JobTitle;
        }

        public DateTime StartDate => _startDate;

        public string JobTitle => _jobTitle;

        internal JobState GetJobState(PersonState personState)
        {
            return new JobState
            {
                Id = _id,
                StartDate = _startDate,
                EndDate = _endDate,
                JobTitle = _jobTitle,
                PersonState = personState
            };
        }

        internal void ChangeJobTitle(string newJobTitle)
        {
            _jobTitle = newJobTitle;
        }

        internal void EndJob(DateTime endDate)
        {
            _endDate = endDate;
        }
    }
}