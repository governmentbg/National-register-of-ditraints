export default class Password {
    userId: string | (string | null)[];
    token: string | (string | null)[];
    newPassword!: string;
    newPasswordConfirm!: string;
    email!: string;

    constructor() {
        this.newPassword = '';
        this.newPasswordConfirm = '';
        this.userId = '';
        this.token = '';
        this.email = '';
    }
}