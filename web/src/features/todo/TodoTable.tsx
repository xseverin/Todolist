import React from 'react';
import TodoRow from './TodoRow';
import TodoForm from "./TodoForm.tsx";

interface Todo {
    id: string;
    name: string;
    done: boolean;
}
interface TodoTableProps {
    todos: Todo[];
    fetchTodos: () => void;
    addTodo: (name: string) => void;
}
const TodoTable: React.FC<TodoTableProps> = ({ todos, fetchTodos, addTodo }) => {
    return (
        
            <table className="transition duration-300 ease-out hover:ease-in">
                <tbody>
                {todos.map(todo => (
                    <TodoRow key={todo.id} todo={todo} fetchTodos={fetchTodos}/>
                ))}

  

                <TodoForm addTodo={addTodo}/>
                </tbody>
            </table>
    );
};

export default TodoTable;
