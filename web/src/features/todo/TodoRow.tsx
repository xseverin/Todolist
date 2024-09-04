import React, { useState } from 'react';

import { deleteTodoApi, toggleTodoApi, shareTodoApi } from '../todo/todoAPI.ts';
import EditTodoForm from './EditTodoForm.tsx';
import {App} from "antd";
import deleteIcon from "../../img/delete.png";
import shareIcon from "../../img/share.png";
interface Todo {
    id: string;
    name: string;
    done: boolean;
}

interface TodoRowProps {
    todo: Todo;
    fetchTodos: () => void;
}

const TodoRow: React.FC<TodoRowProps> = ({ todo, fetchTodos }) => {
    const [checked, setChecked] = useState(todo.done);
    const [isSharing, setIsSharing] = useState(false);
    const [email, setEmail] = useState<string>('');
    const { message } = App.useApp();

    const toggleDone = async () => {
        setChecked(!checked);
        await toggleTodoApi(todo.id, checked).then(() => fetchTodos());
    };

    const deleteTodo = async () => {
        await deleteTodoApi(todo.id).then(() => fetchTodos());
    };
    

    const Sharing = () => {
        setIsSharing(!isSharing);
    };

    const shareTodo = async () => {
        if (email) {
            await shareTodoApi(email, todo.id).catch(() => {message.error("Todo sharing failed");});
            setIsSharing(false);
        } else {
            alert('Please enter an email address.');
        }
    };

    const [isHovered, setIsHovered] = useState(false);
    const handleMouseEnter = () => {
        setIsHovered(true);
    };

    const handleMouseLeave = () => {
        setIsHovered(false);
    };
    function processHover(Sharing, isHovered){
        if(isHovered){
            return (
                <>
                    <button className="btn btn-secondary" onClick={Sharing}><img src={shareIcon} alt="Share"
                                                                                 className="h-4"
                    /></button>
                </>
            )
        } else {
            return (
                <></>
            )
        }
    };
    return (
        <tr  onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave} className="relative">
            <td >
                
                <input
                    type="checkbox"
                    checked={todo.done}
                    onChange={toggleDone}
                />
            </td>
            
            <td>
                    <EditTodoForm todo={todo}/>
            </td>
            
            <td> {isHovered ? (
                <button className="btn btn-danger" onClick={deleteTodo}><img src={deleteIcon} alt="Delete" className="h-4"
                                                                             /></button>) : null}
            </td>
            <td>
            {isSharing && isHovered ? (
                    <>
                        <input
                            type="email"
                            placeholder="Enter email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                        <button className="btn btn-secondary" onClick={shareTodo}><img src={shareIcon} alt="Share" className="h-4"
                        />
                        </button>
                    </>
            ) : (
                processHover(Sharing, isHovered)


            )}
            </td>
        </tr>
    );
};

export default TodoRow;