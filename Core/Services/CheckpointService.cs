//using Core.Entities.Checkpoints;
//using Core.Repositories;

//namespace Core.Services
//{
//    public class CheckpointService
//    {
//        private readonly ICheckpointRepository _checkpointRepository;

//        public CheckpointService(ICheckpointRepository checkpointRepository)
//        {
//            _checkpointRepository = checkpointRepository;
//        }

//        public int Add(Checkpoint checkpoint)
//        {
//            return _checkpointRepository.AddCheckpoint(checkpoint);
//        }

//        public bool Update(Checkpoint checkpoint)
//        {
//            return _checkpointRepository.UpdateCheckpoint(checkpoint);
//        }

//        public bool Delete(Checkpoint checkpoint)
//        {
//            return _checkpointRepository.DeleteCheckpoint(checkpoint);
//        }

//        public Checkpoint Get(int checkpointId)
//        {
//            return _checkpointRepository.GetCheckpoint(checkpointId);
//        }
//    }
//}