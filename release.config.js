const serviceName = 'smokeyweed/getweed.backend';

class SemanticReleaseError extends Error {
    constructor(message, code, details) {
        super(message);
        Error.captureStackTrace(this, this.constructor);
        this.name = 'SemanticReleaseError';
        this.details = details;
        this.code = code;
        this.semanticRelease = true;
    }
}

module.exports = {
    verifyConditions: [
        "@semantic-release/github"
    ],
    prepare: [
        {
            path: "@semantic-release/exec",
            cmd: `docker pull ${serviceName}:${process.env.GITHUB_SHA}`
        },
        {
            path: "@semantic-release/exec",
            cmd: `docker tag ${serviceName}:${process.env.GITHUB_SHA} ${serviceName}:\${nextRelease.version}`
        },
        {
            path: "@semantic-release/exec",
            cmd: `docker tag ${serviceName}:${process.env.GITHUB_SHA} ${serviceName}:latest`
        }
    ],
    publish: [
        {
            path: "@semantic-release/exec",
            cmd: `docker push ${serviceName}:\${nextRelease.version}`
        },
        {
            path: "@semantic-release/exec",
            cmd: `docker push ${serviceName}:latest`
        },
        "@semantic-release/github"
    ]
};
