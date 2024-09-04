import React, { useState, FormEvent } from 'react';

interface TodoFormProps {
    addTodo: (name: string) => void;
}

const TodoForm: React.FC<TodoFormProps> = ({ addTodo }) => {
    const [name, setName] = useState('');

    const handleSubmit = (e: FormEvent) => {
        e.preventDefault();
        addTodo(name);
        setName('');
    };

    return (
        <tr> <td><input
            type="checkbox"/>
        </td><td>
            <form onSubmit={handleSubmit} style={{display: 'inline'}}>
                <input
                    type="text"
                    id="name"
                    name="name"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    required/>
            </form>
        </td>
        </tr>
    );
};

export default TodoForm;
