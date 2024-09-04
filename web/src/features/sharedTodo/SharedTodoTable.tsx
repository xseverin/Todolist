import React from 'react';
import SharedTodoRow from './SharedTodoRow.tsx';

interface Todo {
    id: string;
    name: string;
    done: boolean;
}

interface TodoTableProps {
    todos: Todo[];
    fetchTodos: () => void;
}
const SharedTodoTable: React.FC<TodoTableProps> = ({ todos, fetchTodos }) => {
    return (
        <table>
            <tbody>
            {todos.map(todo => (
                <SharedTodoRow key={todo.id} todo={todo} fetchTodos={fetchTodos} />
            ))}
            </tbody>
        </table>
    );
};

export default SharedTodoTable;
