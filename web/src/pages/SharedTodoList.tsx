import React, { useEffect, useState } from 'react';
import { fetchSharedApi } from "../features/todo/todoAPI.ts";

interface SharedTodo {
    sharedByUserEmail: string;
    sharedWithUserEmail: string;
    todoName: string;
}

export const SharedTodoList = () => {
    const [sharedTodos, setSharedTodos] = useState<SharedTodo[]>([]);

    useEffect(() => {
        const fetchSharedTodos = async () => {
            try {
                const todos = await fetchSharedApi();
                const formattedTodos: SharedTodo[] = todos.map(todo => ({
                    sharedByUserEmail: todo.sharedByUserEmail,
                    sharedWithUserEmail: todo.sharedWithUserEmail,
                    todoName: todo.todoName
                }));
                setSharedTodos(formattedTodos);
                console.log('Fetched todos:', formattedTodos);
            } catch (error) {
                console.error('Error fetching todos:', error);
            }
        };

        fetchSharedTodos();
    }, []);

    return (
        <div className="container mx-auto p-4">
            <h2 className="text-2xl font-bold mb-4">Shared Todo List</h2>
            <div className="overflow-x-auto">
                <table className="min-w-full bg-white border border-gray-200">
                    <thead>
                    <tr>
                        <th className="px-4 py-2 border-b">#</th>
                        <th className="px-4 py-2 border-b">Author</th>
                        <th className="px-4 py-2 border-b">Todo Name</th>
                    </tr>
                    </thead>
                    <tbody>
                    {sharedTodos.map((st, index) => (
                        <tr key={index} className="hover:bg-gray-100">
                            <td className="px-4 py-2 border-b">{index + 1}</td>
                            <td className="px-4 py-2 border-b">{st.sharedByUserEmail}</td>
                            <td className="px-4 py-2 border-b">{st.todoName}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default SharedTodoList;