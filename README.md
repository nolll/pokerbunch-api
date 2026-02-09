# pokerbunch

Backend for pokerbunch.com

## Local development

- Make sure docker is running, and run `docker compose up` to setup the database
- Open backend solution in vs and hit F5
- Login with a user: `admin`, `manager` or `player`. The password is `abcd`

### Database scaffolding

`cd Infrastructure.Sql`

`dotnet ef dbcontext scaffold "Host=localhost:5431;Username=postgres;Password=example" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --no-pluralize --no-onconfiguring --data-annotations --context PokerBunchDbContext`

### Emails

Some actions sends emails. For that to work, install smtp4dev
`dotnet tool install -g Rnwood.Smtp4dev` and run it with `smtp4dev`
