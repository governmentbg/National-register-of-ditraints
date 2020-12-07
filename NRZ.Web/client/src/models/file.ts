export default class FileModel {
    constructor(obj: any = {}) {
        if (obj.file instanceof File) {
            this.file = obj.file;
        } else {
            this.file = new File([], obj.file.fileName);
        }
        this.id = obj.id;
    }

    file: File;
    id: number
}