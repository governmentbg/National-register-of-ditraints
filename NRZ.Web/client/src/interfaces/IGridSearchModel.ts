import { DateTimeFormat } from 'vue-i18n';

export interface IGridSearchModel {
    search: string,
    sortBy: string,
    sortDesc: boolean,
    page: number,
    itemsPerPage: number,
    startDate: Date,
    endDate: Date
}