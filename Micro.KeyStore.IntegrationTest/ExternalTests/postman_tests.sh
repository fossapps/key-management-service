#!/usr/bin/env bash
echo '{"values": [{"key": "url","value": "http://localhost:5000"}]}' > env.json
npx newman run https://www.getpostman.com/collections/4d12132daa3db9eecad2 -e ./env.json && rm env.json
