using Core.Repositories;

namespace Core.UseCases
{
    public class JoinBunchForm
    {
        private readonly IBunchRepository _bunchRepository;

        public JoinBunchForm(IBunchRepository bunchRepository)
        {
            _bunchRepository = bunchRepository;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);

            return new Result(bunch.DisplayName);
        }

        public class Request
        {
            public string Slug { get; }

            public Request(string slug)
            {
                Slug = slug;
            }
        }

        public class Result
        {
            public string BunchName { get; private set; }

            public Result(string bunchName)
            {
                BunchName = bunchName;
            }
        }
    }
}