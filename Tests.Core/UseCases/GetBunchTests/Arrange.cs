﻿using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using Moq;
using NUnit.Framework;

namespace Tests.Core.UseCases.GetBunchTests
{
    public abstract class Arrange
    {
        private const int BunchId = 1;
        private const int UserId = 4;
        private const int PlayerId = 5;
        private const string Slug = "slug";
        private const string UserName = "username";
        protected const string DisplayName = "displayname";
        protected const string Description = "description";
        protected const string HouseRules = "houserules";
        protected virtual Role Role => Role.None;
        protected GetBunch Sut;

        [SetUp]
        public void Setup()
        {
            var bsm = new Mock<IBunchRepository>();
            bsm.Setup(s => s.GetBySlug(Slug)).Returns(new Bunch(BunchId, Slug, DisplayName, Description, HouseRules));

            var prm = new Mock<IPlayerRepository>();
            prm.Setup(s => s.Get(BunchId, UserId)).Returns(new Player(BunchId, PlayerId, UserId, role: Role));

            var urm = new Mock<IUserRepository>();
            urm.Setup(s => s.Get(UserName)).Returns(new User(UserId, UserName));

            Sut = new GetBunch(bsm.Object, urm.Object, prm.Object);
        }

        protected GetBunch.Request Request => new GetBunch.Request(UserName, Slug);
    }
}
