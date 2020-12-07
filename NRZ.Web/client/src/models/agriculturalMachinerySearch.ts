import { IAgriculturalMachinerySearch } from "@/interfaces/agriculturalMachinerySearch";

export default class AgriculturalMachinerySearch implements IAgriculturalMachinerySearch{
    constructor(obj: any) {
        this.page = obj.page;
        this.itemsPerPage = obj.itemsPerPage;
        this.searchString = obj.searchString || "";
        this.sortBy = obj.sortBy || "";
        this.sortDesc = obj.sorDesc || false;
    }
    page: number;
    itemsPerPage: number;
    searchString: string;
    sortBy: string;
    sortDesc: boolean;
}