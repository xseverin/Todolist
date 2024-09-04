import React, { useState } from 'react';
import { deleteTodoApi, toggleTodoApi, shareTodoApi } from '../user/authAPI.ts';
import EditTodoForm from './EdtiTodoForm.tsx';

interface Todo {
    id: string;
    name: string;
    done: boolean;
}

interface TodoRowProps {
    todo: Todo;
    fetchTodos: () => void;
}

const SharedTodoRow: React.FC<TodoRowProps> = () => {


    return (
        <tr>
            <td style={{ textDecoration: todo.done ? 'line-through' : 'none' }}>
                {isEditing ? (
                    <EditTodoForm todo={todo} onSave={stopEditing} onCancel={() => setIsEditing(false)} />
                ) : (
                    todo.name
                )}
            </td>
            <td>
                <input
                    type="checkbox"
                    checked={todo.done}
                    onChange={toggleDone}
                />
            </td>
            <td>
                <button className="btn btn-info" onClick={startEditing}>Edit</button>
            </td>
            <td>
                <button className="btn btn-danger" onClick={deleteTodo}>Delete</button>
            </td>
            <td>
                {isSharing ? (
                    <>
                        <input
                            type="email"
                            placeholder="Enter email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                        <button className="btn btn-secondary" onClick={shareTodo}>Share</button>
                    </>
                ) : (
                    <button className="btn btn-secondary" onClick={startSharing}>Share</button>
                )}
            </td>
        </tr>
    );
};

export default SharedTodoRow;