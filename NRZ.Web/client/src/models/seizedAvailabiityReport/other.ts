export default class Other {
    identifier: string;
    type: string;
    description: string;

    constructor(obj: any = {}) {
        this.identifier = obj.identifier || '';
        this.type = obj.type || '';
        this.description = obj.description || '';
    }
}
