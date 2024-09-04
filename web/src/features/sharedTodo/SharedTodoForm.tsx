import React, { useState, FormEvent } from 'react';

interface TodoFormProps {
    addTodoApi: (name: string) => void;
}

const SharedTodoForm: React.FC<TodoFormProps> = ({ addTodoApi }) => {
    const [name, setName] = useState('');

    const handleSubmit = (e: FormEvent) => {
        e.preventDefault();
        addTodoApi(name);
        setName('');
    };

    return (
        <form onSubmit={handleSubmit} style={{ display: 'inline' }}>
            <label htmlFor="name">Todo Name:</label>
            <input
                type="text"
                id="name"
                name="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
            />
            <button type="submit" className="btn btn-success">Add Todo</button>
        </form>
    );
};

export default SharedTodoForm;
