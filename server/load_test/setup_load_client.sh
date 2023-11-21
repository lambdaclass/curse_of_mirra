export MIX_ENV=prod
cd /tmp
# Only clone `server/` subdirectory
git clone https://github.com/lambdaclass/curse_of_myrra.git dark_worlds_server
cd dark_worlds_server/
git checkout
cd server/load_test

mix local.hex --force && mix local.rebar --force
mix deps.get --only $MIX_ENV
mix deps.compile
mix assets.deploy
mix compile
mix phx.gen.release
mix release

rm -rf /root/dark_worlds_server
mv /tmp/dark_worlds_server /root/
