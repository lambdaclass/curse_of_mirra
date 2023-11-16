# LoadTest

This directory contains the code we use to load test our backend architechture. The organization is as follows:

- This README will contain an explanation on how to run the load tests
- On `reports/` we'll keep track record of the times we run load tests. The methodology, results and actions taken based on it.

```bash
cd server/load_test
mix deps.get
export SERVER_HOST=10.150.20.186:4000
iex -S mix
```

Inside the Elixir shell

```
LoadTest.PlayerSupervisor.spawn_players(50)
```

to create 50 players that will connect to the server and wait to be assigned a
game and then wait for it to start. Once it starts they send random commands
every 30 ms. They still receive updates form the server but those are just
ignored.

If you plan on creating more than 150 players, first increase the file descriptor limit of your shell by doing

```bash
ulimit -n 65535
before running iex -S mix
```

## Analyzing results

If you want to see a request tracking report for every player of every game after a load test, you can run the helper function `DarkWorldsServer.Engine.RequestTracker.report/1` *on the server*. Running

```
DarkWorldsServer.Engine.RequestTracker.report(:game)
```

will show something like this:

```
Report of request tracking
--------------------------
total msgs: 286929
total games: 50

Details per game
------------------
<0.1220.0> =>
   total msgs: 5743
   total players: 3
<0.1296.0> =>
   total msgs: 5730
   total players: 3
```

while running

```
DarkWorldsServer.Engine.RequestTracker.report(:player)
```

will show something like this:

```
Report of request tracking
--------------------------
total msgs: 286929
total games: 2

Details per game
------------------
<0.1220.0> =>
   total msgs: 5743
   total players: 3
   msgs per player =>
       player 1, total msg: 1914
       player 2, total msg: 1915
       player 3, total msg: 1914
<0.1296.0> =>
   total msgs: 5730
   total players: 3
   msgs per player =>
       player 1, total msg: 1910
       player 2, total msg: 1910
       player 3, total msg: 1910
```

Some metrics are also reported to newrelic. Currently %CPU, %RAM are recorded.
We also sent custom metrics which you can find on newrelic by the name
`Custom/CurrentGamesCount`, `Custom/CurrentMessagesCount` and
`Custom/CurrentPlayersCount`. So see how these are calculated or add new custom
metrics, check `DarkWorldsServer.Metrics.CustomMetricsGenerator`.
