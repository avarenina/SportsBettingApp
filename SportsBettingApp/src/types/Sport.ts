import Tip from "@/types/Tip";

export default interface Sport {
    id: number;
    name: string;
    icon: string;
    availableTips: Tip[];
}