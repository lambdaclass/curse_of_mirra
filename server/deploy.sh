#!/usr/bin/env bash
. "$HOME/.asdf/asdf.sh"
set -ex

if [ -d "/tmp/dark_worlds_server" ]; then
  rm -rf /tmp/dark_worlds_server
fi

cd /tmp
git clone git@github.com:lambdaclass/dark_worlds_server.git --branch ${BRANCH}
cd dark_worlds_server/server

mix local.hex --force && mix local.rebar --force
mix deps.get --only $MIX_ENV
mix deps.compile
mix assets.deploy
mix compile
mix phx.gen.release
mix release

rm -rf /root/dark_worlds_server
mv /tmp/dark_worlds_server /root/

cat <<EOF > /etc/systemd/system/dark_worlds_server.service
[Unit]
Description=Dark Worlds server
Requires=network-online.target
After=network-online.target

[Service]
User=root
WorkingDirectory=/root/dark_worlds_server/server
Restart=on-failure
ExecStart=/root/dark_worlds_server/server/entrypoint.sh
ExecReload=/bin/kill -HUP
KillSignal=SIGTERM
EnvironmentFile=/root/.env

[Install]
WantedBy=multi-user.target
EOF

systemctl enable dark_worlds_server

cat <<EOF > /root/.env
PHX_HOST=${PHX_HOST}
PHX_SERVER=${PHX_SERVER}
SECRET_KEY_BASE=${SECRET_KEY_BASE}
DATABASE_URL=${DATABASE_URL}
EOF

systemctl stop dark_worlds_server

/root/dark_worlds_server/server/_build/prod/rel/dark_worlds_server/bin/migrate

systemctl daemon-reload
systemctl start dark_worlds_server
