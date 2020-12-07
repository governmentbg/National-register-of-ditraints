import { IGridSearchModel } from '@/interfaces/IGridSearchModel';

export default class GridSearchModel implements IGridSearchModel {
    constructor(search: string, sortBy: string, sortDesc: boolean, page: number, itemsPerPage: number, startDate: Date, endDate: Date) {
        this.search = search,
        this.sortBy = sortBy,
        this.sortDesc = sortDesc,
        this.page = page,
        this.itemsPerPage = itemsPerPage
        this.startDate = startDate,
        this.endDate = endDate;
    }
    search: string;
    sortBy: string;
    sortDesc: boolean;
    page: number;
    itemsPerPage: number;
    startDate: Date;
    endDate: Date;
}