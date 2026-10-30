import type { Customer } from "../types/customer";
import { CustomerCard } from "./CustomerCard";

interface CustomerGridProps {
    customers: Customer[];
}

export function CustomerGrid({ customers }: CustomerGridProps) {
    return (
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {customers.map((customer) => (
                <CustomerCard key={customer.id} customer={customer} />
            ))}
        </div>
    );
}
