export default class Password {
    userId: string;
    oldPassword!: string;
    newPassword!: string;
    newPasswordConfirm!: string;

    constructor() {
        this.oldPassword = '';
        this.newPassword = '';
        this.newPasswordConfirm = '';
        this.userId = '';
    }
}