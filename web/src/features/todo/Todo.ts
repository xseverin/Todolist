export interface Todo {
    Id: string;
    Name: string;
    Done: boolean;
    ParentId?: string;
    Children?: Todo[];
    Deep?: number;  // Optional, as in the TodoTable type
}