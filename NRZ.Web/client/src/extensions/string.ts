/*eslint prefer-rest-params: "off"*/
interface String {
    format(): string;
}


String.prototype.format = function () {
    let result = this;
    for (let index = 0; index < arguments.length; index++) {
        result = result.replace(`{${index}}`, arguments[index]);
    }

    return result.toString();
};