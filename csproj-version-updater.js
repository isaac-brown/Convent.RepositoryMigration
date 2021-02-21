// Standard version updater for csproj files.
fs = require("fs");

// Matches on a version number wrapped in a <Version> tag.
const versionRegex = /(?<=<Version>).*(?=<\/Version>)/i;

module.exports.readVersion = function (contents) {
  const version = contents.match(versionRegex)[0];
  return version;
};

module.exports.writeVersion = function (contents, version) {
  return contents.replace(versionRegex, version);
};
